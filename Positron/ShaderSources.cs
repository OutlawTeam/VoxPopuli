using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Positron
{
    internal static class ShaderSources
    {
        internal static string Base2DVert = @"
            version 330 core

            layout(location = 0) in vec2 aPosition;
            layout(location = 1) in vec2 aTexCoord;
            layout(location = 2) in vec4 aColor;
            out vec2 vTexCoord;
            out vec4 vColor;
            uniform mat4 projection;
            void main()
            {
                gl_Position = projection * vec4(aPosition, 0.0, 1.0);
                vTexCoord = aTexCoord;
                vColor = aColor.xyzw;
            }
        ";
        internal static string Base2DFrag = @"
            #version 330 core

            in vec2 vTexCoord;
            in vec4 vColor;
            out vec4 FragColor;



            uniform sampler2D textureSampler;

            void main()
            {
                FragColor = vColor * texture(textureSampler, vTexCoord);
            }
        ";
        internal static string BaseFontVert = @"
            #version 460

            layout (location = 0) in vec2 in_pos;
            layout (location = 1) in vec2 in_uv;

            out vec2 vUV;

            layout (location = 0) uniform mat4 model;
            layout (location = 1) uniform mat4 projection;

            void main()
            {
                vUV         = in_uv.xy;
                gl_Position = projection * model * vec4(in_pos.xy, 0.0, 1.0);
            }
        ";
        internal static string BaseFontFrag = @"
            #version 460

            in vec2 vUV;

            layout (binding=0) uniform sampler2D text;

            layout (location = 2) uniform vec3 textColor;

            out vec4 fragColor;

            void main()
            {
                vec4 sampled = vec4(1.0, 1.0, 1.0, texture(text, vUV).r);
                fragColor = vec4(textColor, 1.0) * sampled;
            }
        ";
    }
}
