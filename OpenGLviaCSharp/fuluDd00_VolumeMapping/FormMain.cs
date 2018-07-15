﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fuluDd00_VolumeMapping
{
    public partial class FormMain : Form
    {

        private Scene scene;
        private ActionList actionList;
        private PeelingNode peelingNode;

        public FormMain()
        {
            InitializeComponent();

            this.Load += FormMain_Load;
            this.winGLCanvas1.OpenGLDraw += winGLCanvas1_OpenGLDraw;
            this.winGLCanvas1.Resize += winGLCanvas1_Resize;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            var position = new vec3(5, 3, 4) * 1f;
            var center = new vec3(0, 0, 0);
            var up = new vec3(0, 1, 0);
            var camera = new Camera(position, center, up, CameraType.Perspecitive, this.winGLCanvas1.Width, this.winGLCanvas1.Height);
            var scene = new Scene(camera);
            var rootElement = GetTree(scene);
            scene.RootNode = rootElement;
            scene.ClearColor = Color.SkyBlue.ToVec4();
            this.scene = scene;

            var list = new ActionList();
            var transformAction = new TransformAction(scene.RootNode);
            list.Add(transformAction);
            var renderAction = new RenderAction(scene);
            list.Add(renderAction);
            this.actionList = list;

            // Note: uncomment this to enable camera movement.
            //var manipulater = new FirstPerspectiveManipulater();
            //manipulater.StepLength = 0.1f;
            //manipulater.Bind(camera, this.winGLCanvas1);
        }

        private SceneNodeBase GetTree(Scene scene)
        {
            var children = new List<SceneNodeBase>();
            {
                const float alpha = 0.2f;
                var colors = new vec4[] { new vec4(1, 0, 0, alpha), new vec4(0, 1, 0, alpha), new vec4(0, 0, 1, alpha) };

                for (int k = -1; k < 2; k++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        int index = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            vec3 worldPosition = new vec3(i * 2, j * 2, k * 2);
                            var cubeNode = CubeNode.Create(new CubeModel(), CubeModel.positions);
                            cubeNode.WorldPosition = worldPosition;
                            cubeNode.Color = colors[index++];
                            cubeNode.Name = string.Format("{0},{1},{2}:{3}", k, j, i, cubeNode.Color);

                            children.Add(cubeNode);
                        }
                    }
                }
            }
            this.peelingNode = new PeelingNode(children.ToArray());

            return this.peelingNode;
        }

        private void winGLCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            ActionList list = this.actionList;
            if (list != null)
            {
                vec4 clearColor = this.scene.ClearColor;
                GL.Instance.ClearColor(clearColor.x, clearColor.y, clearColor.z, clearColor.w);
                GL.Instance.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

                list.Act(new ActionParams(Viewport.GetCurrent()));
            }
        }

        void winGLCanvas1_Resize(object sender, EventArgs e)
        {
            this.scene.Camera.AspectRatio = ((float)this.winGLCanvas1.Width) / ((float)this.winGLCanvas1.Height);
        }

    }
}
