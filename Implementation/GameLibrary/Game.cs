using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary {
  public enum GameState {
    LOADING,
    TITLE_SCREEN,
    ON_MAP,
    FIGHTING,
    DEAD,
    LVL2,
    LVL1,
    BOSS,
    WIN,
    MENU,
    }

  public class Game {
    private static Game game;

    public Character Character { get; private set; }
    public GameState State { get; private set; }

    public Inventory Inventory { get; private set; }

    private Game() {
      State = GameState.LOADING;
    }

    public static Game GetGame() {
      if (game == null)
        game = new Game();
      return game;
    }

    public void ChangeState(GameState newState) {
      State = newState;
    }

    public void SetCharacter(Character character) {
      Character = character;
    }
    //Set this instance's inventory
    public void SetInventory(Inventory inventory) {
        Inventory = inventory;
    }
  }
}
