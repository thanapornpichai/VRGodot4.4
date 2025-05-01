extends MeshInstance3D

@export var beam_mesh      : MeshInstance3D
@export var end_particles  : GPUParticles3D
@export var rayCast : RayCast3D
@export var rayCastPiVot : Node3D
@export var hit_vfx: Node3D
var pivot_locked := false

var start_locked := false
var start_world  : Vector3

@export var m_rot_degrees := 45
var current_degrees := 0
var last_hit_local  : Vector3

var tween : Tween

func _process(_delta):
	rayCast.force_raycast_update()

	if rayCast.is_colliding():
		var hit_world = rayCast.get_collision_point()
		print(hit_world)
		print(to_local(hit_world))

		if !start_locked:
			start_locked = true
			start_world = global_transform.origin

		var end_local = to_local(hit_world)
		laser_activate(end_local, 0.15)
	else:
		var max_reach_world = global_transform.origin + global_transform.basis.y * 100.0
		var end_local = to_local(max_reach_world)
		laser_activate(end_local, 0.05)
		
	if Input.is_action_pressed("Naitest3") and current_degrees <  m_rot_degrees:
		current_degrees += 45
		update_rotation()

	if Input.is_action_pressed("Naitest4") and current_degrees > -m_rot_degrees:
		current_degrees -= 45
		update_rotation()
	

func update_rotation():
	rotation_degrees.y = current_degrees
	
func laser_activate(hit_local: Vector3, time := 0.15):
	# hit_local.y คือตำแหน่งตามแกน Y ของ RayCast
	beam_mesh.mesh.height   = hit_local.y           # ยืดความสูงทรงกระบอก
	beam_mesh.position.y    = hit_local.y * 0.5     # ขยับขึ้นครึ่งหนึ่งให้ฐานอยู่ที่ 0
	end_particles.position  = hit_local           # วางปลาย effect ตรงจุดชน
	
	tween = get_tree().create_tween().set_parallel(true)
	
	visible = true
	#end_particles.emitting = true                  # เปิดพาร์ทิเคิลถ้าต้องการ
	
	tween.tween_property(beam_mesh.mesh, "top_radius",    0.10, time)
	tween.tween_property(beam_mesh.mesh, "bottom_radius", 0.10, time)
	tween.tween_property(end_particles.process_material, "scale_min", 1.0,  time)
	await tween.finished

func deactivate(time := 0.15):
	if !visible:
		return
	tween = get_tree().create_tween().set_parallel(true)
	tween.tween_property(beam_mesh.mesh, "top_radius",    0.0, time)
	tween.tween_property(beam_mesh.mesh, "bottom_radius", 0.0, time)
	tween.tween_property(end_particles.process_material, "scale_min", 0.0, time)
	await tween.finished
	visible = false
	end_particles.emitting = false
	
func OnLaserHit(hit_pos : Vector3, dir : Vector3):
	# ล็อก pivot แค่ครั้งแรก
	rayCast.global_position = hit_pos   # จุดกำเนิดใหม่
	hit_vfx.global_position = hit_pos
	pivot_locked = true
	
	# หมุนตามทิศสะท้อนทุกครั้ง
	rayCast.look_at(hit_pos + dir, Vector3.UP)
	rayCast.rotate_object_local(Vector3.RIGHT, deg_to_rad(90))

	hit_vfx.look_at(hit_vfx.global_position + dir, Vector3.UP)
