using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BaseVoxelTextureFolder = "assets/textures/blocks";
        public static Texture GetAtlas()
        {
            return VoxelAtlas;
        }
        public static float[] GetAtlasTextures(string Name)
        {
            if (BlockAtlasTexture.TryGetValue(Name, out var Texture))
            {
                return Texture;
            }
            else
            {
                throw new Exception("The demanded texture coordinates doesn't existe :" + Name);
            }
        }
        private static void LoadVoxelAtlas()
        {
            
            List<Image<Rgba32>> loadedTextures = new List<Image<Rgba32>>();
            List<string> textureNames = new List<string>();

            string[] blockTextures = Directory.GetFiles(BaseVoxelTextureFolder, "*.png");
            foreach (string filePath in blockTextures)
            {
                if (Path.GetFileNameWithoutExtension(filePath) != "atlas")
                {
                    using var stream = File.OpenRead(filePath);
                    var texture = Image.Load<Rgba32>(stream);
                    loadedTextures.Add(texture);
                    textureNames.Add(Path.GetFileNameWithoutExtension(filePath));
                }
            }
            foreach (string mod in ModManager.ModAssetFolder)
            {
                string[] blocksTextures = Directory.GetFiles(mod+"/"+BaseVoxelTextureFolder, "*.png");
                foreach (string filePath in blocksTextures)
                {
                    if (Path.GetFileNameWithoutExtension(filePath) != "atlas")
                    {
                        using var stream = File.OpenRead(filePath);
                        var texture = Image.Load<Rgba32>(stream);
                        loadedTextures.Add(texture);
                        textureNames.Add(Path.GetFileNameWithoutExtension(filePath));
                    }
                }
            }

            int numTextures = loadedTextures.Count;
            int maxTextureWidth = loadedTextures.Max(t => t.Width);
            int maxTextureHeight = loadedTextures.Max(t => t.Height);

            int atlasSize = Math.Max(maxTextureWidth, maxTextureHeight) * (int)Math.Ceiling(Math.Sqrt(numTextures));

            var atlasImage = new Image<Rgba32>(atlasSize, atlasSize);

            int currentX = 0;
            int currentY = 0;

            for (int i = 0; i < numTextures; i++)
            {
                var texture = loadedTextures[i];

                if (currentX + texture.Width > atlasSize)
                {
                    currentX = 0;
                    currentY += maxTextureHeight;
                }
                // Create texture coordinates for the current texture
                float left = (float)currentX / atlasSize;
                float right = (float)(currentX + texture.Width) / atlasSize;
                float top = (float)currentY / atlasSize;
                float bottom = (float)(currentY + texture.Height) / atlasSize;

                float[] coordinates = new float[8];
                coordinates[0] = left;
                coordinates[1] = top;
                coordinates[2] = left;
                coordinates[3] = bottom;
                coordinates[4] = right;
                coordinates[5] = bottom;
                coordinates[6] = right;
                coordinates[7] = top;


                // Add texture coordinates to the dictionary with the file name as the key
                BlockAtlasTexture.Add(textureNames[i], coordinates);

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        var pixel = texture[x, y];
                        atlasImage[currentX + x, currentY + y] = pixel;
                    }
                }

                currentX += maxTextureWidth;
            }

            // Sauvegarder l'image de l'atlas dans un fichier PNG
            atlasImage.Save("debug/atlas.png");
            VoxelAtlas = Texture.LoadFromData(atlasImage);
        }
    }
}
