﻿/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System;
using StateMasher;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infiniminer.States
{
    public class LoadingState : State
    {
        Texture2D texMenu;
        Rectangle drawRect;
        string nextState = null;
        SpriteFont uiFont;
        string[] currentHint;

        static string[] HINTS = new string[18]
        {
            "Engineers can build bank blocks near ore veins for\nMiners to quickly fill the team's supplies.",
            "Sappers can use TNT to excavate around gold and\ndiamonds, as it does not destroy them.",
            "Gold occurs in veins that can run dozens of blocks\nin length; follow the veins!",
            "You can paste a server name or IP into the direct\nconnect field by using Ctrl-V.",
            "The Engineer's jump blocks cost as much as a ladder\nblock but are far more efficient.",
            "Beacon blocks are shown on your teammates' radar.\nUse them to mark important locations.",
            "Build force fields to keep the enemy out of your tunnels.",
            "Shock blocks will kill anyone who touches their underside.",
            "Combine jump blocks and shock blocks to make deadly traps!",
            "The Prospectron 3000 can detect gold and diamonds through walls.\nLet a prospector guide you when digging.",
            "Miners can dig much faster than the other classes!\nUse them to quickly mine out an area.",
            "Engineers can build force fields of the other team's color.\nUse this ability to create bridges only accessible to your team.",
            "Movement speed is doubled on road blocks.\nUse them to cover horizontal distances quickly.",
            "Return gold and diamonds to the surface to collect loot for your team!",
            "Press Q to quickly signal your teammates.",
            "All constructions require metal ore.\nDig for some or take it from your team's banks.",
            "Don't have a scroll wheel?\nPress R to cycle through block types for the construction gun.",
            "You can set your name and adjust your screen resolution\nby editing the client configuration file.",
        };

        public override void OnEnter(string oldState)
        {
            _SM.IsMouseVisible = false;

            texMenu = _SM.Content.Load<Texture2D>("menus/tex_menu_loading");

            drawRect = new Rectangle(_SM.GraphicsDevice.Viewport.Width / 2 - 1024 / 2,
                                     _SM.GraphicsDevice.Viewport.Height / 2 - 768 / 2,
                                     1024,
                                     1024);

            uiFont = _SM.Content.Load<SpriteFont>("font_04b08");

            // Pick a random hint.
            Random randGen = new Random();
            currentHint = HINTS[randGen.Next(0, HINTS.Length)].Split("\n".ToCharArray());
        }

        public override void OnLeave(string newState)
        {

        }

        public override string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            // Do network stuff.
            (_SM as InfiniminerGame).UpdateNetwork(gameTime);

            return nextState;
        }

        public override void OnRenderAtEnter(GraphicsDevice graphicsDevice)
        {

        }

        public override void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            uint dataPacketsRecieved = 0;
            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y += 16)
                    if (_P.mapLoadProgress[x, y])
                        dataPacketsRecieved += 1;
            string progressText = String.Format("{0:00}% LOADED", dataPacketsRecieved / 256.0f * 100);

            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
            spriteBatch.Draw(texMenu, drawRect, Color.White);
            spriteBatch.DrawString(uiFont, progressText, new Vector2(((int)(_SM.GraphicsDevice.Viewport.Width / 2 - uiFont.MeasureString(progressText).X / 2)), drawRect.Y + 430), Color.White);
            for (int i = 0; i < currentHint.Length; i++)
                spriteBatch.DrawString(uiFont, currentHint[i], new Vector2(((int)(_SM.GraphicsDevice.Viewport.Width / 2 - uiFont.MeasureString(currentHint[i]).X / 2)), drawRect.Y + 600 + 25 * i), Color.White);
            spriteBatch.End();
        }

        public override void OnKeyDown(Keys key)
        {
            if (key == Keys.Escape)
            {
                _P.netClient.Disconnect("Client disconnected.");
                nextState = "Infiniminer.States.ServerBrowserState";
            }
        }

        public override void OnKeyUp(Keys key)
        {

        }

        public override void OnMouseDown(MouseButton button, int x, int y)
        {

        }

        public override void OnMouseUp(MouseButton button, int x, int y)
        {

        }

        public override void OnMouseScroll(int scrollDelta)
        {

        }
    }
}
