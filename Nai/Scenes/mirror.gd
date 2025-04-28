extends MeshInstance3D

@export var m_rot_degrees = 45
var current_degrees = 0

func _process(delta):
	if Input.is_action_pressed("Naitest3"):
		if current_degrees < m_rot_degrees:
			current_degrees += 45
			update_rotation()
	
	if Input.is_action_pressed("Naitest4"):
		if current_degrees > -m_rot_degrees:
			current_degrees -= 45
			update_rotation()

func update_rotation():
	rotation_degrees.y = current_degrees
