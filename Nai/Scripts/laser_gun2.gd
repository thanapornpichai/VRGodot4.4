extends RayCast3D

@onready var beam_mesh      : MeshInstance3D = $BeamMesh
@onready var end_particles  : GPUParticles3D = $EndParticles
@export var hit_vfx: Node3D

var tween : Tween

func _process(_delta: float) -> void:
	# ให้ RayCast คำนวณใหม่ทุกเฟรม
	force_raycast_update()
	
	if is_colliding():
		
		var incident : Vector3 = (get_collision_point() - global_transform.origin).normalized()
		var normal   : Vector3 = get_collision_normal().normalized()

		var reflect  : Vector3 = incident.reflect(normal)   # เวกเตอร์สะท้อน
		var angle_deg: float   = rad_to_deg(incident.angle_to(reflect))  # มุมสะท้อนเป็นองศา
		hit_vfx.global_position = get_collision_point()
		hit_vfx.look_at(hit_vfx.global_position + reflect, Vector3.UP)

		print("มุมสะท้อน =", angle_deg)

		var hit_local : Vector3 = to_local(get_collision_point())
		laser_activate(hit_local)

# ----------------- เลเซอร์ -----------------
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
