﻿using System.Collections.Generic;
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Tiles;
using KoreDefenceGodot.Core.Scripts.Player;
using KoreDefenceGodot.Core.Scripts.Tower;
using Path = KoreDefenceGodot.Core.Scripts.Engine.Tiles.Path;

namespace KoreDefenceGodot.Core.Scripts.Engine.Game
{
    public class GameInfo : Node
    {
        public static Color InvalidColour = new Color(255, 0, 0, 0.5f);
        public static Color ValidColour = new Color(255, 255, 255, 0.5f);
        public List<BaseEnemy> EnemyList { get; set; }
        public static List<BaseTower> TowerList { get; set; }

        // TODO : Implement towers + tower list
        /// <summary>
        ///     List of all tiles, needed for lava collisions
        /// </summary>
        public Tile[,] TileList { get; set; }

        public static Path GamePath { get; set; }

        // TODO Impelement currency
        public Player.Player Player { get; set; }

        // TODO Implement tile finder
        // TODO Implement tower remover?
        public int MoneySpawnCount { get; set; } = 0;

        /// <summary>
        ///     Should be from 1 - 7. Represents each "level" of the game
        /// </summary>
        public static int LevelNumber { get; set; }

        /// <summary>
        ///     Should be from 1-3. Each "level" has 3 "waves"
        /// </summary>
        public static int WaveNumber { get; set; } = 1;

        public PlayerBase PlayerBase { get; set; }

        // flags used for achievements
        public bool HasUsedSlownessFlag { get; set; } = false;
        public bool ExpensiveTowerDestroyed { get; set; } = false;

        /// <summary>
        ///     Sets the raw wave number.
        ///     Finds level number from wave number e.g 20 will return 7,
        ///     Then finds wave number from raw wave number e.g. 20 will return 2, 21 will return 3.
        /// </summary>
        /// <param name="rawWaveNumber"></param>
        public void SetRawWaveNumber(int rawWaveNumber)
        {
            LevelNumber = (rawWaveNumber - 1 - (rawWaveNumber - 1) % 3) / 3 + 1;
            WaveNumber = (rawWaveNumber - 1) % 3 + 1;
        }

        public static int GetRawWaveNumber()
        {
            return LevelNumber * 3 + WaveNumber;
        }


        /// <summary>
        ///     Get bounding rectangle of an animated sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static Rect2 GetRect(AnimatedSprite sprite)
        {
            // rectangle of the current frame of the animated sprite
            var texture = sprite.Frames.GetFrame(sprite.Animation, sprite.Frame);
            return new Rect2(sprite.Position - texture.GetSize() / 2, texture.GetSize());
        }
    }
}