#define PLAY_IMPLEMENTATION
#define PLAY_USING_GAMEOBJECT_MANAGER
#include "Play.h"		//The playbuffer header file
#include <iostream>		//File saving headers. Used for saving the highscore when game is closed
#include <fstream>
#include <string>
#include <dos.h>		//Used for adding delay to the game

//Declaring the screen size
int DISPLAY_WIDTH = 450 * 2;		
int DISPLAY_HEIGHT = 500 * 2;
int DISPLAY_SCALE = 1;

//Variable for the highscore
int highscore;

//Creating a GameState for player
enum PlayerState
{
	STATE_PLAY = 0,
	STATE_DEATH,
};

PlayerState playerstate;
Vector2D startingpos;

//Creating the state for the game
struct GameState				
{
	int score = 0;
	int lives = 3;
};

GameState gameState;

//Setting all of the game object types starting at 0
enum GameObjectType		
{
	TYPE_NULL = -1,		//This represents all uninitialised game objects to reduce erorrs
	TYPE_PLAYER,
	TYPE_BULLET,
	TYPE_ENEMY = 2,
	TYPE_EBULLET,
	TYPE_SUPERENEMY,
	TYPE_POWERUP,
};

//The state the game is currently in
enum GameScreen
{
	STATE_MENU,
	STATE_GAME,
	STATE_HIGHSCORE,
};

GameScreen gamescreen;
bool isplaying;			//Variables to help the menu selection
bool ismenu;	

int enemycounter;	//Variable for the second enemy
Vector2D enemypos; //Variable for the enemy position for power ups
int multiplier = 1;    //Variable for the score mulitplier
int multicounter;

//Forward declaration of functions
void RetrieveingData();
void SavingData();
void MenuSetUp();
void GameSetUp();
void HighscoreSetUp();
void EnemyCreation();
void SuperEnemyCreation();
void PlayerMovement();
void UpdateBullets();
void UpdateEnemy();
void UpdateUI();
void CreatePowerUps();
void UpdatePowerUps();
void ResetMulti();


// The entry point for a PlayBuffer program
void MainGameEntry(PLAY_IGNORE_COMMAND_LINE)
{
	RetrieveingData();			//Retriving the current highscore

	Play::CreateManager(DISPLAY_WIDTH, DISPLAY_HEIGHT, DISPLAY_SCALE);
	MenuSetUp();
}

// Called by PlayBuffer every frame (60 times a second!)
bool MainGameUpdate(float elapsedTime)
{
	Play::ClearDrawingBuffer(Play::cGrey);
	Play::DrawBackground();			//Drawing the background

	if (Play::KeyPressed(0x4D) && ismenu == true) {		//If m is pressed
		MenuSetUp();
	}
	if (Play::KeyPressed(0x50) && isplaying == false) {		//If p is pressed
		GameSetUp();
		isplaying = true;		//Setting the play variable to be true
	}
	
	//Switch case for which gamescreen we are currently on
	switch (gamescreen) {
		case STATE_MENU:
		{
			MenuSetUp();
			ismenu = false;
			break;
		}
		case STATE_GAME:
		{
			PlayerMovement();					//Updating the player
			UpdateBullets();
			UpdateEnemy();
			UpdateUI();
			UpdatePowerUps();
			ResetMulti();
			break;
		}
		case STATE_HIGHSCORE:
		{
			HighscoreSetUp();
			break;
		}
	}
	Play::PresentDrawingBuffer();
	return Play::KeyDown(VK_ESCAPE);	//Calling MainGameExit when escape is pressed
}

// Gets called once when the player quits the game 
int MainGameExit(void)
{
	SavingData();				//Saving the current highscore
	
	Play::DestroyManager();
	
	return PLAY_OK;
}


//Function for saving game information
void SavingData() {
	//Saving highscore to a file
	std::ofstream gameInfo("GameInfo.txt");
	if (gameInfo.is_open())
	{
		gameInfo << highscore;
		gameInfo.close();
	}
}
//Function for loading gaming information
void RetrieveingData() {
	//Collecting highscore from the game file
	std::string line;
	std::ifstream gameInfo("GameInfo.txt");
	if (gameInfo.is_open())
	{
		while (getline(gameInfo, line))
		{
			highscore = std::stoi(line);
		}
		gameInfo.close();
	}
	else {
		highscore = 0;
	}
}
//Function for creation the menu set up
void MenuSetUp() {
	gamescreen = STATE_MENU;
	playerstate = STATE_PLAY;
	isplaying = false;
	//Reseting the gamestate variables
	gameState.lives = 3;
	gameState.score = 0;
	multiplier = 1;

	std::vector<int> vEnemy = Play::CollectGameObjectIDsByType(TYPE_ENEMY);		//Clearing the previous enemies
	for (int id_enemy : vEnemy)
	{
		Play::DestroyGameObject(id_enemy);		

	}
	
	Play::CentreAllSpriteOrigins();
	Play::LoadBackground("Data\\Backgrounds\\spaceBackground.png");									//Preloading the background
	Play::DrawFontText("32px", "Space Invaders", { DISPLAY_WIDTH / 3, DISPLAY_HEIGHT / 5 });
	Play::DrawFontText("64px", "Press p to start the game", { DISPLAY_WIDTH / 3, DISPLAY_HEIGHT / 2 });
}
//Function for creation the game set up
void GameSetUp() {
	gamescreen = STATE_GAME;
	Play::CentreAllSpriteOrigins();
	Play::LoadBackground("Data\\Backgrounds\\spaceBackground.png");									//Preloading the background
	Play::StartAudioLoop("background");											//Starting the background music
	Play::CreateGameObject(TYPE_PLAYER, { DISPLAY_WIDTH / 2, DISPLAY_HEIGHT - 100 }, 25, "playerShip");		//Creating memory for the sprites
	startingpos = { DISPLAY_WIDTH / 2, DISPLAY_HEIGHT - 100 };
	EnemyCreation();
}
//Function for the creation of the highscore screen
void HighscoreSetUp() {
	gamescreen = STATE_HIGHSCORE;
	Play::DrawFontText("32px", "Highscore Board", { DISPLAY_WIDTH / 3, DISPLAY_HEIGHT / 5 });
	Play::DrawFontText("64px", "The current highscore is: " + std::to_string(highscore), {DISPLAY_WIDTH / 3, DISPLAY_HEIGHT / 2});
	Play::DrawFontText("64px", "Press m to go back to the menu", { DISPLAY_WIDTH / 3, 800 });
	Play::StopAudioLoop("background");											//Stopping the background music
	ismenu = true;
	enemycounter = 0;				//Reseting the second enemy counter
}
//Function for the creation of enemies
void EnemyCreation() {
	for (int i = 0; i < 10; i++) {
		for (int j = 0; j < 3; j++) {
			Play::CreateGameObject(TYPE_ENEMY, Vector2D(200 + (i * 50), 200 + (j * 45)), 5, "Enemy");	//Make sure that integers are not used after sprite names as it divides the sprite by the integer used. Eg. a 2 will half the sprite
		}
	}
	std::vector<int> vEnemy1 = Play::CollectGameObjectIDsByType(TYPE_ENEMY);

	for (int id_enemy1 : vEnemy1)
	{
		GameObject& enemy = Play::GetGameObject(id_enemy1);			//Enemy set up
		enemy.velocity.x = 4;
		enemy.radius = 10;
	}
}
//Function for creating the super enemy
void SuperEnemyCreation()  {
	if (Play::RandomRoll(1000) == 1) {					//Randomly spawns a super enemy
		int enemy2_id = Play::CreateGameObject(TYPE_SUPERENEMY, Vector2D(10, 100), 10, "SuperEnemy");
		GameObject& enemy2 = Play::GetGameObject(enemy2_id);
		enemy2.velocity.x = 4;
	}
}
//Function for all player movements
void PlayerMovement()
{
	GameObject& player = Play::GetGameObjectByType(TYPE_PLAYER);

	switch (playerstate) {
	case STATE_PLAY: {
		if (Play::IsLeavingDisplayArea(player, Play::HORIZONTAL)) {			//Checking to see if the player has moved off the screen
			player.velocity = { 0, 0 };
			player.pos = player.oldPos;										//Stopping them from moving off the screen
		}
		else {
			if (Play::KeyDown(VK_RIGHT)) {									//Allowing player to move freely on the horizontal axis
				player.velocity = { 4, 0 };
			}
			else if (Play::KeyDown(VK_LEFT)) {
				player.velocity = { -4, 0 };
			}
			else {
				player.velocity *= 0.9f;
			}
		}
		if (Play::KeyPressed(VK_SPACE))
		{
			Play::SetSprite(player, "fire", 0.1f);							//Animation for firing bullets

			GameObject& bullet = Play::GetGameObjectByType(TYPE_BULLET);
			Vector2D firePos1 = player.pos + Vector2D(40, -85);
			Vector2D firePos2 = player.pos + Vector2D(-40, -85);
			Play::CreateGameObject(TYPE_BULLET, firePos1, 5, "Bullet");
			Play::CreateGameObject(TYPE_BULLET, firePos2, 5, "Bullet");
			Play::PlayAudio("shot");	//Playing a gun shot sound
		}
		if (Play::IsAnimationComplete(player)) {							//Animation check to stop the animation
			Play::SetSprite(player, "playerShip", 0);
		}
		break;
	}
	case STATE_DEATH: {
		Play::SetSprite(player, "death", 0.1f);								//Death animation
		if (Play::IsAnimationComplete(player)) {							//Checks if the player has finished animating ready for next life
			Play::SetSprite(player, "playerShip", 0);
			playerstate = STATE_PLAY;
			player.pos = startingpos;
		}
		if (Play::IsAnimationComplete(player)) {							//Check to send player to the highscore screen
			if(gameState.lives == 0)
			gamescreen = STATE_HIGHSCORE;
		}
		break;
	}
	}
	
	Play::UpdateGameObject(player);
	Play::DrawObject(player);
}
//Function for updating all of the bullets on the screen
void UpdateBullets()
{
	std::vector<int> vBullet = Play::CollectGameObjectIDsByType(TYPE_BULLET);
	std::vector<int> vEnemy = Play::CollectGameObjectIDsByType(TYPE_ENEMY);
	std::vector<int> vEbullet = Play::CollectGameObjectIDsByType(TYPE_EBULLET);

	//Player bullets code
	for (int id_Bullet : vBullet)		
	{
		GameObject& bullet = Play::GetGameObject(id_Bullet);
		bool hasCollided = false;
		bullet.velocity = { 0,-16 };

		for (int id_enemy : vEnemy)					//Enemy1 death when colliding with bullet
		{
			GameObject& enemy = Play::GetGameObject(id_enemy);
			if (Play::IsColliding(bullet, enemy))
			{
				hasCollided = true;
				enemypos = enemy.pos;						//Used for spawning powerup
				Play::DestroyGameObject(id_enemy);
				CreatePowerUps();
				gameState.score += 300 * multiplier;
			}
		}
		GameObject& enemy2 = Play::GetGameObjectByType(TYPE_SUPERENEMY);	//Enemy2 death when colliding with bullet
		int id_enemy2 = enemy2.GetId();
		if (Play::IsColliding(bullet, enemy2))
		{
			hasCollided = true;
			Play::DestroyGameObject(id_enemy2);
			gameState.score += 1000 * multiplier;
		}

		Play::UpdateGameObject(bullet);
		Play::DrawObject(bullet);

		if (!Play::IsVisible(bullet) || hasCollided)			//Destroying used bullets
			Play::DestroyGameObject(id_Bullet);
	}
	//Enemy bullets code
	for (int id_Ebullet : vEbullet)			
	{
		GameObject& Ebullet = Play::GetGameObject(id_Ebullet);
		bool hasECollided = false;
		Ebullet.velocity = { 0,16 };

		GameObject& player = Play::GetGameObjectByType(TYPE_PLAYER);
		if (Play::IsColliding(Ebullet, player))
		{
			hasECollided = true;
			gameState.lives -= 1;
			playerstate = STATE_DEATH;				//Changing the player to death state
		}

		Play::UpdateGameObject(Ebullet);
		Play::DrawObject(Ebullet);

		if (!Play::IsVisible(Ebullet) || hasECollided)
			Play::DestroyGameObject(id_Ebullet);
	}		
}
//Function for the enemies
void UpdateEnemy()
{
	//Functionality for the main enemy
	std::vector<int> vEnemy = Play::CollectGameObjectIDsByType(TYPE_ENEMY);
	if (vEnemy.empty()) {
		EnemyCreation();			//Creation of more enemies once all are destroyed
	}
	else {
		SuperEnemyCreation();			//Chance to spawn super enemy
		for (int id_enemy : vEnemy)
		{
			GameObject& enemy = Play::GetGameObject(id_enemy);
			if (enemy.type == TYPE_NULL) return;

			if (Play::IsLeavingDisplayArea(enemy))			//Making the enemies increase in speed as they move down the screen
			{
				enemy.pos = enemy.oldPos;
				if (enemy.velocity.x < 8) {			//Capping the max speed at 8
					enemy.velocity.x  *= -1.1;
					enemy.pos.y += 20;
				}
				else {
					enemy.velocity.x *= -1;
					enemy.pos.y += 20;
				}
			}
			if (Play::RandomRoll(1000) == 1) {
				GameObject& ebullet = Play::GetGameObjectByType(TYPE_EBULLET);
				Vector2D firePos = enemy.pos + Vector2D(20, 20);
				Play::CreateGameObject(TYPE_EBULLET, firePos, 5, "Bullet");
			}
			GameObject& player = Play::GetGameObjectByType(TYPE_PLAYER);
			if (Play::IsColliding(player, enemy))				//Checking the the enemies have made it to the bottom of the level
			{
				gameState.lives = 0;
				playerstate = STATE_DEATH;
			}
			Play::UpdateGameObject(enemy);
			Play::DrawObject(enemy);
		}
	}

	//Functionality for the super enemy
	GameObject& enemy2 = Play::GetGameObjectByType(TYPE_SUPERENEMY);
	int id_enemy2 = enemy2.GetId();
	if (enemy2.type == TYPE_NULL) return;

	if (enemycounter != 3) {							//Checking to see if the super enemy has been on the screen for 3 rotations
		if (Play::IsLeavingDisplayArea(enemy2))
		{
			enemy2.pos = enemy2.oldPos;
			enemy2.velocity.x *= -1.3;
			enemycounter += 1;
		}
		Play::UpdateGameObject(enemy2);
		Play::DrawObject(enemy2);
	}
	else {
		if (Play::IsVisible(enemy2))
		{
			//Play::DestroyGameObjectsByType(TYPE_SUPERENEMY);			//First attempt didnt work
			Play::DestroyGameObject(id_enemy2);							//Correct code for second attempt
			enemycounter = 0;
		}
	}
	if (Play::RandomRoll(100) == 1) {
		GameObject& ebullet = Play::GetGameObjectByType(TYPE_EBULLET);
		Vector2D firePos = enemy2.pos + Vector2D(20, 20);
		Play::CreateGameObject(TYPE_EBULLET, firePos, 5, "Bullet");
	}
}
//Function to update the UI that appears on the screen
void UpdateUI()
{
	//Drawing the UI on the screen
	Play::DrawFontText("64px", "Score: " + std::to_string(gameState.score), { DISPLAY_WIDTH / 20, DISPLAY_HEIGHT / 20 });
	Play::DrawFontText("64px", "Highscore: " + std::to_string(highscore), { DISPLAY_WIDTH / 2, DISPLAY_HEIGHT / 20 });
	Play::DrawFontText("64px", "x" + std::to_string(multiplier), { DISPLAY_WIDTH / 3, DISPLAY_HEIGHT / 20 });

	//Updating the highscore counter
	if (gameState.score > highscore) {
		highscore = gameState.score;
	}

	//Updating the lives counter
	switch (gameState.lives) {
		case 0:
		{
			break;
		}
		case 1:
		{
			Play::DrawSprite("Lives", { 20, 970 }, 0);
			break;
		}
		case 2:
		{
			Play::DrawSprite("Lives", { 20, 970 }, 0);
			Play::DrawSprite("Lives", { 50, 970 }, 0);
			break;
		}
		case 3:
		{
			Play::DrawSprite("Lives", { 20, 970}, 0);
			Play::DrawSprite("Lives", { 50, 970 }, 0);
			Play::DrawSprite("Lives", { 80, 970 }, 0);
			break;
		}
	}
}
//Function for the powerups
void CreatePowerUps() {
	if (Play::RandomRoll(15) == 1) {				//Random chance to spawn a powerup
 		Play::CreateGameObject(TYPE_POWERUP, enemypos, 5, "PowerUp");
		
		std::vector<int> PowerUp = Play::CollectGameObjectIDsByType(TYPE_POWERUP);

		for (int id_powerup : PowerUp)
		{
			GameObject& powerup = Play::GetGameObject(id_powerup);
			powerup.velocity.y = 4;
		}
	}
}
//Function to update the powerups
void UpdatePowerUps() {
	std::vector<int> PowerUps = Play::CollectGameObjectIDsByType(TYPE_POWERUP);
	for (int id_powerup : PowerUps)
	{
		GameObject& powerup = Play::GetGameObject(id_powerup);
		if (powerup.type == TYPE_NULL) return;

		GameObject& player = Play::GetGameObjectByType(TYPE_PLAYER);
		if (Play::IsColliding(powerup, player))
		{
			Play::DestroyGameObject(id_powerup);
			multiplier = 2;							//Setting a multiplier
			multicounter = 0;
			Play::PlayAudio("powerup");	//Playing a powerup pickup sound
			break;
		}
		Play::UpdateGameObject(powerup);
		Play::DrawObject(powerup);
	}	
}
//Function to turn multiplyer back to normal
void ResetMulti() {
	if (multicounter == 600) {			//Creation of a counter to perform a simple time delay for the powerup. 600 should be about 10 seconds
		multiplier = 1;
		Play::PlayAudio("losepowerup");		//Playing a powerup lose sound
	}
	multicounter += 1;
}