using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packets;

namespace Client
{
    public class Game : MonoGame.Forms.Controls.MonoGameControl
    {
        Client GetClient;
        Texture2D whiteDraft;
        Texture2D board;

        Vector2 WhitePiecePos;

        public bool hostTurn;

        public Game(Client _gameClient)
        {
            GetClient = _gameClient;
            Console.WriteLine("new game");
        }

        protected override void Initialize()
        {
            base.Initialize();
            hostTurn = true;
            whiteDraft = Editor.Content.Load<Texture2D>("Images/ball");
            board = Editor.Content.Load<Texture2D>("Images/CheckersBoard");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GetClient.isHost)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    WhitePiecePos.X = PointToClient(MousePosition).X;
                    WhitePiecePos.Y = PointToClient(MousePosition).Y;
                    MovePacket sendMove = new MovePacket(WhitePiecePos.X, WhitePiecePos.Y);
                    GetClient.UDPSendMessage(sendMove);
                }
            }
        }

        public void MovePiece(int x, int y)
        {
            WhitePiecePos = new Vector2(x, y);
        }

        protected override void Draw()
        {
            base.Draw();
            Editor.spriteBatch.Begin();

            Editor.spriteBatch.Draw(board, new Vector2(75, 20),
                Color.White);

            Editor.spriteBatch.Draw(whiteDraft, WhitePiecePos,
                Color.White);

            Editor.spriteBatch.End();
        }
    }
}