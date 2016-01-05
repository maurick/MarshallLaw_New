using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class MapLoader
    {
        Vector2 mapDimensions;
        Vector2 tileDimensions;
        string orientation;

        List<Layer> layers;
        List<string> spritesheets;

        int numberOfLayers;

        enum state
        {
            header, layer, end
        }

        enum headerstate
        {
            header, width, height, tilewidth, tileheight, orientation, tileset, tilsetsheet
        }

        state currentState;
        headerstate currentHeaderState;

        public MapLoader()
        {
            layers = new List<Layer>();
            spritesheets = new List<string>();
        }

        public string LoadMap(string filename)
        {

            var path = @"Content\Maps\" + filename + ".txt";

            StreamReader sr = new StreamReader(path);

            string buffer = "";

            while (true)
            {
                switch (currentState)
                {
                    case state.header:
                        #region "Read Header"
                        switch (currentHeaderState)
                        {
                            case headerstate.header:
                                buffer = sr.ReadLine();
                                if (buffer != "[header]")
                                {
                                    return "Error1";
                                }
                                currentHeaderState++;
                                break;
                            case headerstate.width:
                                buffer = sr.ReadLine();
                                buffer = buffer.Remove(0, 6);
                                if (Convert.ToInt32(buffer) <= 0 )
                                {
                                    return "Error2";
                                }
                                mapDimensions.X = Convert.ToInt32(buffer);

                                currentHeaderState++;
                                break;
                            case headerstate.height:
                                buffer = sr.ReadLine();
                                buffer = buffer.Remove(0, 7);
                                if (Convert.ToInt32(buffer) <= 0)
                                {
                                    return "Error3";
                                }
                                mapDimensions.Y = Convert.ToInt32(buffer);

                                currentHeaderState++;
                                break;
                            case headerstate.tilewidth:
                                buffer = sr.ReadLine();
                                buffer = buffer.Remove(0, 10);
                                if (Convert.ToInt32(buffer) != 32)
                                {
                                    return "Error4";
                                }
                                tileDimensions.X = Convert.ToInt32(buffer);

                                currentHeaderState++;
                                break;
                            case headerstate.tileheight:
                                buffer = sr.ReadLine();
                                buffer = buffer.Remove(0, 11);
                                if (Convert.ToInt32(buffer) != 32)
                                {
                                    return "Error5";
                                }
                                tileDimensions.Y = Convert.ToInt32(buffer);

                                currentHeaderState++;
                                break;
                            case headerstate.orientation:
                                buffer = sr.ReadLine();
                                buffer = buffer.Remove(0, 12);
                                if (buffer != "orthogonal")
                                {
                                    return "Error6";
                                }
                                orientation = buffer;

                                currentHeaderState++;
                                break;
                            case headerstate.tileset:
                                buffer = sr.ReadLine();
                                if (buffer == "[tilesets]")
                                {
                                    currentHeaderState++;
                                }
                                break;
                            case headerstate.tilsetsheet:
                                buffer = sr.ReadLine();

                                if (buffer == "")
                                {
                                    currentState++;
                                    break;
                                }

                                if (buffer != "tileset=,1,1,0,0")
                                {
                                    int pngPos = buffer.IndexOf(".png");
                                    int lastDashPos = buffer.LastIndexOf('/');

                                    buffer = buffer.Substring(lastDashPos + 1, pngPos - lastDashPos - 1);

                                    spritesheets.Add(buffer);
                                }

                                break;
                        }
                        #endregion
                        break;
                    case state.layer:
                        #region "Read Layer"

                        buffer = sr.ReadLine();

                        buffer = sr.ReadLine();

                        buffer = buffer.Remove(0, 5);

                        //GameSettings.Instance.Tilescale = new Vector2(GameSettings.Instance.Dimensions.X / mapDimensions.X , GameSettings.Instance.Dimensions.Y / mapDimensions.Y);
                        GameSettings.Instance.Tilescale = new Vector2(24, 24);

                        Layer tempLayer = new Layer(buffer, mapDimensions, (int)mapDimensions.X, (int)mapDimensions.Y);

                        buffer = sr.ReadLine();

                        for (int y = 0; y < mapDimensions.Y; y++)
                        {
                            int x = 0;
                            buffer = sr.ReadLine();
                            char[] tempChararray = { ',' };
                            string[] tile_ids = buffer.Split(tempChararray, count: (int)mapDimensions.X);

                            while (x < mapDimensions.X)
                            { 
                                if (x == mapDimensions.X - 1 && y != mapDimensions.Y - 1)
                                {
                                    tile_ids[x] = tile_ids[x].Remove(tile_ids[x].IndexOf(','));
                                }

                                if (tile_ids[x].Contains("\r\n"))
                                {
                                    tile_ids[x] = tile_ids[x].Remove(0, 2);
                                }

                                int temp_tileID = Convert.ToInt32(tile_ids[x]);

                                tempLayer.AddTile(x, y, temp_tileID);
                                x++;

                            }
                        }
                        //Lege regel nog effe lezen zodat de pointer verzet wordt
                        sr.ReadLine();

                        layers.Add(tempLayer);
                        numberOfLayers++;

                        if (sr.EndOfStream)
                        {
                           return "Load Complete";
                        }
                        #endregion
                        break;
                }
            }
        }

        public List<Layer> GetLayers()
        {
            return layers;
        }

        public Vector2 GetMapDimensions()
        {
            return mapDimensions;
        }

        public int GetNumLayers()
        {
            return numberOfLayers;
        }

        public List<string> GetSpritesheetList()
        {
            return spritesheets;
        }

        public string GetOrientation()
        {
            return orientation;
        }
    }
    
}
