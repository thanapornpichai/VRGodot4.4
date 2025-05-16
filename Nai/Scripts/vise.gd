extends MeshInstance3D

@export var rot_speed = 5
@export var m_rot = 15       
var c_rot = 0                

func _process(delta):
	if Input.is_action_pressed("Naitest3") and c_rot < m_rot:
		var step = rot_speed * delta
		var new_rot = min(c_rot + step, m_rot)
		rotate_y(deg_to_rad(new_rot - c_rot))
		c_rot = new_rot

	if Input.is_action_pressed("Naitest4") and c_rot > -m_rot:
		var step = rot_speed * delta
		var new_rot = max(c_rot - step, -m_rot)
		rotate_y(deg_to_rad(new_rot - c_rot))
		c_rot = new_rot
