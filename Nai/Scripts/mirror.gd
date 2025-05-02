extends MeshInstance3D

@export var m_rot_degrees := 45
var current_degrees := 0
var last_hit_local  : Vector3

func register_hit_point(hit_global: Vector3) -> void:
	last_hit_local = to_local(hit_global)
	print("โดนยิงที่ (local): ", last_hit_local)

func _process(_delta):
	if Input.is_action_pressed("Naitest3") and current_degrees <  m_rot_degrees:
		current_degrees += 45
		update_rotation()

	if Input.is_action_pressed("Naitest4") and current_degrees > -m_rot_degrees:
		current_degrees -= 45
		update_rotation()

func update_rotation():
	rotation_degrees.y = current_degrees
