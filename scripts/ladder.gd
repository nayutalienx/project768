extends Node2D

signal ladder_join

func _on_area_2d_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		ladder_join.emit(true, get_global_position())


func _on_area_2d_body_exited(body: Node2D) -> void:
	if body.is_in_group("player"):
		ladder_join.emit(false, get_global_position())
