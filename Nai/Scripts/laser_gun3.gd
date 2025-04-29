extends RayCast3D

@export var hit_vfx: Node3D
@onready var beam_mesh: MeshInstance3D = $BeamMesh
@onready var end_particles: GPUParticles3D = $EndParticles

const MAX_BOUNCE := 3   # จำนวนครั้งที่ชิ่ง

var tween: Tween

func _process(_delta: float) -> void:
	force_raycast_update()
	if is_colliding():
		var hit_point = get_collision_point()
		var origin = global_transform.origin
		var direction = (hit_point - origin).normalized()
		shoot_bounce(origin, direction, MAX_BOUNCE)

# ยิงลำแสงและสะท้อนตามจำนวน bounce_left
func shoot_bounce(from: Vector3, direction: Vector3, bounce_left: int):
	if bounce_left <= 0:
		return
	
	# สร้าง RayCast3D ชั่วคราว
	var ray := RayCast3D.new()
	ray.target_position = direction * 1000.0
	ray.global_transform.origin = from + direction * 0.01
	add_child(ray)
	ray.force_raycast_update()

	if ray.is_colliding():
		var hit = ray.get_collision_point()
		var normal = ray.get_collision_normal().normalized()
		var reflect_dir = direction.reflect(normal)

		# สร้าง hit_vfx ที่ตำแหน่งชน
		var vfx = hit_vfx.duplicate()
		get_tree().current_scene.add_child(vfx)
		vfx.global_position = hit
		vfx.look_at(hit + reflect_dir, Vector3.UP)

		# ยิงเด้งต่อ
		shoot_bounce(hit, reflect_dir, bounce_left - 1)

	ray.queue_free()
