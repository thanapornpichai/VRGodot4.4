extends RayCast3D

@onready var beam_mesh = $BeamMesh
@onready var end_particles = $EndParticles

var tween: Tween

func _ready() -> void:
	pass

func  _process(delta: float) -> void:
	var cast_point
	force_raycast_update()
	if Input.is_action_pressed("Naitest1"):
		deactivate()
	elif Input.is_action_pressed("Naitest2"):
		if is_colliding():
			cast_point = to_local(get_collision_point())
			laseractivate(cast_point)
		
func laseractivate(cast_point, time=1):
	beam_mesh.mesh.height = cast_point.y
	beam_mesh.position.y = cast_point.y
	print(cast_point.y / 2)
	#end_particles.position.y = cast_point.y

	tween = get_tree().create_tween()
	visible = true
	#end_particles.emitting = true
	tween.set_parallel(true)

	tween.tween_property(beam_mesh.mesh, "top_radius", 0.1, time)
	tween.tween_property(beam_mesh.mesh, "bottom_radius", 0.1, time)

	tween.tween_property(end_particles.process_material, "scale_min", 1, time)
	await tween.finished

func deactivate(time=1):
	tween = get_tree().create_tween()
	tween.set_parallel(true)
	tween.tween_property(beam_mesh.mesh, "top_radius", 0.0, time)
	tween.tween_property(beam_mesh.mesh, "bottom_radius", 0.0, time)
	tween.tween_property(end_particles.process_material, "scale_min", 0.0, time)
	await tween.finished
	visible = false
	end_particles.emitting = false
