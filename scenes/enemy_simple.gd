extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0

var enemy_direction: int = 1

@onready var fall_raycast_1 = $FallRaycast_1
@onready var fall_raycast_2 = $FallRaycast_2


func _physics_process(delta: float) -> void:
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	#if Input.is_action_just_pressed("ui_accept") and is_on_floor():
	#	velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	
	if not fall_raycast_1.is_colliding() or not fall_raycast_2.is_colliding() or is_on_wall():
		if is_on_floor():
			enemy_direction *= -1
	
	velocity.x = enemy_direction * SPEED

	move_and_slide()
