﻿using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Roystan/Post Process Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings
{
    public IntParameter scale = new IntParameter { value = 1 };
    public FloatParameter depthThreshold = new FloatParameter {value = 0.2f};
    [Range(0,1)]
    public FloatParameter normalThreshold = new FloatParameter {value = 0.4f};

    [Range(0, 1)]
    public FloatParameter depthNormalThreshold = new FloatParameter { value = 0.5f };
    public FloatParameter depthNormalThresholdScale = new FloatParameter { value = 7 };
    public Vector3Parameter cameraCoord = new Vector3Parameter{ value = Vector3.zero };
    public ColorParameter color = new ColorParameter { value = Color.white };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Roystan/Outline Post Process"));
        sheet.properties.SetFloat("_Scale", settings.scale);
        sheet.properties.SetFloat("_DepthThreshold", settings.depthThreshold);
        sheet.properties.SetFloat("_NormalThreshold", settings.normalThreshold);

        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;

        sheet.properties.SetMatrix("_ClipToView", clipToView);
        sheet.properties.SetFloat("_DepthNormalThreshold", settings.depthNormalThreshold);
        sheet.properties.SetFloat("_DepthNormalThresholdScale", settings.depthNormalThresholdScale);
        sheet.properties.SetVector("_cameraCoord", settings.cameraCoord);
        sheet.properties.SetColor("_Color", settings.color);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}