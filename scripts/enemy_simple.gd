extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0

var enemy_direction: int = 1

@onready var fall_raycast_1 = $FallRaycast_1
@onready var fall_raycast_2 = $FallRaycast_2

signal enemy_hit

func _physics_process(delta: float) -> void:
	
	if not is_on_floor():
		velocity += get_gravity() * delta
	
	if not fall_raycast_1.is_colliding() or not fall_raycast_2.is_colliding() or is_on_wall():
		if is_on_floor():
			enemy_direction *= -1
	
	velocity.x = enemy_direction * SPEED

	move_and_slide()


func _on_enemy_attack_area_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		enemy_hit.emit()
