extends MeshInstance3D

@onready var cap = $Cap
var is_open := false

func _ready():
	add_to_group("safe_boxes")

func open_box():
	if is_open:
		return
	is_open = true
	print("SafeBox opened!")
	if cap:
		var tween = get_tree().create_tween()
		tween.tween_property(cap, "rotation_degrees:y", -90.0, 1.0)
