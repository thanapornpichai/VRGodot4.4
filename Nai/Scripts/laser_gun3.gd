extends RayCast3D

@onready var beam_mesh      : MeshInstance3D = $BeamMesh
@onready var end_particles  : GPUParticles3D = $EndParticles

var start_locked := false
var start_world  : Vector3

var tween : Tween

func _process(_delta):
	force_raycast_update()

	if is_colliding():
		var hit_world = get_collision_point()

		if !start_locked:
			start_locked = true
			start_world = global_transform.origin

		var end_local = to_local(hit_world)
		laser_activate(end_local, 0.15)

		var inc  = (hit_world - start_world).normalized()
		var nrm  = get_collision_normal().normalized()
		var refl = inc.reflect(nrm)

		var root = get_collider().get_parent()
		if root and root.has_method("OnLaserHit"):
			root.OnLaserHit(hit_world, refl)

	else:
		# ไม่ชนอะไรเลย → ยิงเต็มระยะไปตาม target_position
		if !start_locked:
			start_locked = true
			start_world = global_transform.origin

		var max_reach_world = global_transform.origin + global_transform.basis.y * 100.0
		var end_local = to_local(max_reach_world)
		laser_activate(end_local, 0.05)

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
