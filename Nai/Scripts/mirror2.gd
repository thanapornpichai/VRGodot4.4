extends MeshInstance3D

@export var beam_mesh      : MeshInstance3D
@export var end_particles  : GPUParticles3D
@export var rayCast        : RayCast3D
@export var hit_vfx        : Node3D

var tween : Tween
var safe_box_hit_time := 0.0
var safe_box : Node = null
var is_open = false

func _process(delta):
	rayCast.force_raycast_update()
	if rayCast.is_colliding():
		var collider = rayCast.get_collider()
		var cap  = collider.get_parent()
		var safebox = collider.get_parent().get_parent()
		var hit_world = rayCast.get_collision_point()
		var end_local = to_local(hit_world)
		
		if is_open:
			deactivate()
		elif not is_open:
			laser_activate(end_local, 0.15)

		if safe_box_hit_time >= 10.0:
			if safe_box and safe_box.has_method("open_box"):
				is_open = true
				print(is_open)
				safe_box.open_box()
				
		if safebox.is_in_group("safe_boxes"):
			safe_box = safebox
			safe_box_hit_time += delta
		else:
			safe_box_hit_time = 0.0

func laser_activate(hit_local: Vector3, time := 0.15):
	beam_mesh.mesh.height = hit_local.y
	beam_mesh.position.y = hit_local.y * 0.5
	end_particles.position = hit_local

	tween = get_tree().create_tween().set_parallel(true)
	visible = true
	tween.tween_property(beam_mesh.mesh, "top_radius", 0.10, time)
	tween.tween_property(beam_mesh.mesh, "bottom_radius", 0.10, time)
	tween.tween_property(end_particles.process_material, "scale_min", 1.0, time)
	await tween.finished

func deactivate(time := 0.15):
	if !visible:
		return
	tween = get_tree().create_tween().set_parallel(true)
	tween.tween_property(beam_mesh.mesh, "top_radius", 0.0, time)
	tween.tween_property(beam_mesh.mesh, "bottom_radius", 0.0, time)
	tween.tween_property(end_particles.process_material, "scale_min", 0.0, time)
	await tween.finished
	visible = false
	end_particles.emitting = false
