extends CharacterBody2D


@export var SPEED = 300.0
@export var JUMP_VELOCITY = -400.0

var killed: bool = false

var is_in_ladder = false
var current_ladder_pos: Vector2

enum PlayerMode {
	GROUND,
	LADDER
}

var playerMode = PlayerMode.GROUND

func _ready():
	var enemies = get_tree().get_nodes_in_group("enemy")
	for enemy in enemies:
		enemy.connect("enemy_hit", _on_enemy_hit)
		enemy.connect("enemy_death", _on_enemy_death)
	
	var ladders = get_tree().get_nodes_in_group("ladder")	
	for ladder in ladders:
		ladder.connect("ladder_join", _on_ladder_react)

func _on_ladder_react(in_ladder, ladder_pos):
	is_in_ladder = in_ladder
	current_ladder_pos = ladder_pos
	if !in_ladder:
		to_ground_mode()

func to_ground_mode():
	playerMode = PlayerMode.GROUND
	
func to_ladder_mode():
	playerMode = PlayerMode.LADDER
	velocity.x = 0
	velocity.y = 0
	position.x = current_ladder_pos.x

func _on_enemy_hit():
	killed = true

func _on_enemy_death():
	velocity.y = JUMP_VELOCITY
	
func _process(_delta: float):
	if killed:
		get_tree().reload_current_scene()

func _physics_process(delta: float) -> void:
	
	match playerMode:
		PlayerMode.LADDER:
			movePlayerOnLadder(delta)
		PlayerMode.GROUND:
			movePlayerOnGround(delta)

func movePlayerOnGround(delta: float):
	
	if is_in_ladder && Input.is_action_just_pressed("ui_up"):
		to_ladder_mode()
		return
	
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("ui_left", "ui_right")
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	move_and_slide()

func movePlayerOnLadder(delta: float):
	
	if (Input.is_action_just_pressed("ui_accept") or 
		Input.is_action_just_pressed("ui_left") or 
		Input.is_action_just_pressed("ui_right")):
		to_ground_mode()
		velocity.y = 0
		velocity.x = 0
		return
	
	var direction := Input.get_axis("ui_up", "ui_down")
	if direction:
		velocity.y = direction * SPEED
	else:
		velocity.y = move_toward(velocity.y, 0, SPEED)

	move_and_slide()
