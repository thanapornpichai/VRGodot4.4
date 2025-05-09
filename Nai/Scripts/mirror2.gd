extends MeshInstance3D

@export var beam_mesh      : MeshInstance3D
@export var end_particles  : GPUParticles3D
@export var rayCast        : RayCast3D
var tween : Tween
var safe_box_hit_time := 0.0
var safe_box : Node = null
var is_open = false
var on_Reflex = false;

func _process(delta):
	if not on_Reflex:
		return
		
	rayCast.exclude_parent = true
	rayCast.force_raycast_update()
	if rayCast.is_colliding():
		var safebox = rayCast.get_collider()
		var hit_world = rayCast.get_collision_point()
		var end_local = to_local(hit_world)
		
		if is_open && !on_Reflex :
			deactivate()
		elif not is_open:
			laser_activate(end_local, 0.15)

		if safe_box_hit_time >= 3.0:
			if safe_box and safe_box.has_method("OpenBox"):
				is_open = true
				StopLaser()
				safe_box.OpenBox()
				
		if safebox.is_in_group("safe_boxes"):
			safe_box = safebox
			safe_box_hit_time += delta
		else:
			safe_box_hit_time = 0.0

func laser_activate(hit_local: Vector3, time := 0.15):
	beam_mesh.mesh.height = hit_local.y
	beam_mesh.position.y = hit_local.y * 0.5

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

func OnLaserHit(hit_pos : Vector3, dir : Vector3):
	on_Reflex = true
	rayCast.global_position = hit_pos
	
	rayCast.look_at(hit_pos + dir, Vector3.UP)
	rayCast.rotate_object_local(Vector3.RIGHT, deg_to_rad(90))
	
	if rayCast.is_colliding():
		end_particles.global_position = rayCast.get_collision_point()
		var hit = rayCast.get_collider()
		print("Laser hit: ", hit.name)
		
func StopLaser():
	on_Reflex = false
