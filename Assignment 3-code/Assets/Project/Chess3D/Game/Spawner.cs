using Assets.Project.Chess3D.Pieces;
using Assets.Project.ChessEngine;
using Assets.Project.ChessEngine.Pieces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Chess3D
{
    public class Spawner: MonoBehaviour
    {
        public List<Transform> piecePrefabs;
        public GameObject Pieces;
        public GameController gc;
        Piece lastRanPiece, secondLastRanPiece;

    

        public void DoMove(Move move)
        {
            if (move.IsEnPassant)
            {
                if (gc.Board.OnTurn == ChessEngine.Color.White)
                {
                    DestroyPiece(gc.Board.Pieces[(int)move.ToSq - 10], true);
                }
                else
                {
                    DestroyPiece(gc.Board.Pieces[(int)move.ToSq + 10], true);
                }
            }
            else if (move.IsCastle)
            {
                switch (move.ToSq)
                {
                    case Square.C1:
                        MovePiece(gc.Board.Pieces[(int)Square.A1], Board.Sq64((int)Square.D1), false);
                        break;
                    case Square.C8:
                        MovePiece(gc.Board.Pieces[(int)Square.A8], Board.Sq64((int)Square.D8), false);
                        break;
                    case Square.G1:
                        MovePiece(gc.Board.Pieces[(int)Square.H1], Board.Sq64((int)Square.F1), false);
                        break;
                    case Square.G8:
                        MovePiece(gc.Board.Pieces[(int)Square.H8], Board.Sq64((int)Square.F8), false);
                        break;
                }
            }

            if (move.CapturedPiece != null)
            {
                DestroyPiece(gc.Board.Pieces[(int)move.ToSq], true);
            }

            if (move.PromotedPiece.HasValue)
            {
                DestroyPiece(gc.Board.Pieces[(int)move.FromSq], false);
            }
            else MovePiece(gc.Board.Pieces[(int)move.FromSq], Board.Sq64((int)move.ToSq), true);
        }

        public PieceWrapper SpawnPiece(Piece piece)
        {
            Vector3 worldPoint = ToWorldPoint(Board.Sq64((int)piece.Square));
            Transform transform = Instantiate(piecePrefabs[piece.Index]);
            transform.position = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
            transform.parent = Pieces.transform;
            transform.localRotation = transform.gameObject.name.Contains("White") ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
            transform.gameObject.SetActive(true);
            PieceWrapper wrapper = transform.GetComponent<PieceWrapper>();
            wrapper.Square = piece.Square;
            //wrapper.moveToPos(worldPoint);
            return wrapper;
        }

        public void DestroyPiece(Piece piece, bool playanim)
        {
            try
            {
                PieceWrapper wrapper = FindPieceWrapper(piece);
                //secondLastRanPiece.Attack();
                if(playanim) FindPieceWrapper(GameController.lastSelectedPiece).Attack();
                //FindPieceWrapper(lastRanPiece).Attack();
                if(playanim) wrapper.Die();
                else Destroy(wrapper.gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(gc.Board.ToString());
                throw e;
            }
        }

        public void MovePiece(Piece piece, int sq64, bool playanim)
        {
            Vector3 worldPoint = ToWorldPoint(sq64);
            PieceWrapper wrapper = FindPieceWrapper(piece);
            wrapper.Square = (Square)Board.Sq120(sq64);
            /*if(!playanim) wrapper.transform.position = new Vector3(worldPoint.x, wrapper.transform.position.y, worldPoint.z);
            else */wrapper.moveToPos(worldPoint);
            secondLastRanPiece = lastRanPiece;
            lastRanPiece = piece;
        }

        public PieceWrapper FindPieceWrapper(Piece piece)
        {
            foreach (Transform child in Pieces.transform)
            {
                PieceWrapper current = child.GetComponent<PieceWrapper>();
                if (current.Square == piece.Square) return current;
            }
            return null;
        }

        private Vector3 ToWorldPoint(int cellNumber)
        {
            int j = cellNumber % 8;
            int i = cellNumber / 8;
            return new Vector3(i * -4 + 14, 1, j * 4 - 14);
        }
    }
}
