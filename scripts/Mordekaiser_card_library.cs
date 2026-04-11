using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using Mordekaiser.Mordekaiserpools;

namespace Mordekaiser.scripts;

[HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary._Ready))]
public class Mordekaiser_library
{
    
    private static readonly MethodInfo? _updateMethod;
    private static readonly FieldInfo? _poolFiltersField;
    private static readonly FieldInfo? _cardPoolDictField;
    private static readonly FieldInfo? _lastHoveredField;

    static Mordekaiser_library()
    {
        Type type = typeof(NCardLibrary);
        _updateMethod = type.GetMethod("UpdateCardPoolFilter", BindingFlags.Instance | BindingFlags.NonPublic);
        _poolFiltersField = type.GetField("_poolFilters", BindingFlags.Instance | BindingFlags.NonPublic);
        _cardPoolDictField = type.GetField("_cardPoolFilters", BindingFlags.Instance | BindingFlags.NonPublic);
        _lastHoveredField = type.GetField("_lastHoveredControl", BindingFlags.Instance | BindingFlags.NonPublic);
    }
    
    [HarmonyPostfix] 
    private static void InjectMordekaiserPool(NCardLibrary __instance)
    {
        static PackedScene poolToggleScene() => GD.Load<PackedScene>("res://scenes/screens/card_library/library_pool_toggle.tscn");
        static Texture2D GetIcon() => GD.Load<Texture2D>("res://images/ui/top_panel/character_icon_mordekaiser.png");
        static Script GetFilterScript() => GD.Load<Script>("res://src/Core/Nodes/Screens/CardLibrary/NCardPoolFilter.cs");
        static Shader GetHsvShader() => GD.Load<Shader>("res://shaders/hsv.gdshader");

        Node poolFilters = __instance.GetNode("Sidebar").GetNode("MarginContainer").GetNode("TopVBox").GetNode("PoolFilters");
        if (poolFilters == null) return;

        if (!poolFilters.HasNode("MordekaiserPool"))
        {
            int insertIndex;
            
            Node defectPool = poolFilters.GetNode("DefectPool");
            
            if (defectPool == null) 
                insertIndex = 1;
            else
                insertIndex = defectPool.GetChildCount() + 1;
            
            
            //创建根节点：%MordekaiserPool (Control)
            //唯一名称(%) 实例化 library_pool_toggle 场景
            Control mordekaiserPool = (Control)poolToggleScene().Instantiate();
            mordekaiserPool.Name = "MordekaiserPool";
            mordekaiserPool.FocusNeighborTop = __instance.GetNode("Sidebar").GetNode("MarginContainer").GetNode("TopVBox").GetNode("SearchBar").GetNode("TextArea").GetPath();
            mordekaiserPool.SetScript(GetFilterScript());

            TextureRect image = mordekaiserPool.GetNode<TextureRect>("Image");

            // Image控件
            image.Texture = GetIcon();
            image.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
            image.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
            image.LayoutMode = 1;
            image.AnchorsPreset = 8;
            
            // HSV 着色器材质
            Shader hsvShader = GetHsvShader();
            ShaderMaterial hsvMaterial = new ShaderMaterial();
            hsvMaterial.Shader = hsvShader;
            image.Material = hsvMaterial;

            //Image 子节点：shadow
            TextureRect shadow = image.GetNode<TextureRect>("Shadow");
            shadow.Texture = GetIcon();
            shadow.LayoutMode = 1;
            shadow.AnchorsPreset = 15;
        
            // 同级 Control
            Control selectionreticle = mordekaiserPool.GetNode<Control>("SelectionReticle");
            selectionreticle.UniqueNameInOwner = true;
            selectionreticle.LayoutMode = 1;
            selectionreticle.AnchorsPreset = 15;
            
            //插入到指定位置（DefectPool 和 ColorlessPool 中间）
            
            poolFilters.AddChild(mordekaiserPool);
            poolFilters.MoveChild(mordekaiserPool, insertIndex);
        
        }
        
        //原版脚本文件对应的需要额外注册的东西
        var action = (Action<NCardPoolFilter>)Delegate.CreateDelegate(typeof(Action<NCardPoolFilter>), __instance, _updateMethod!);
        Callable callable1 = Callable.From(action);
        var _moredekaiserFilter = poolFilters.GetNode<NCardPoolFilter>((NodePath) "MordekaiserPool");
        _moredekaiserFilter.Connect(NCardPoolFilter.SignalName.Toggled, callable1);
        
        Dictionary<NCardPoolFilter, Func<CardModel, bool>> _poolFilters = (Dictionary<NCardPoolFilter, Func<CardModel, bool>>)_poolFiltersField!.GetValue(__instance)!;
        _poolFilters.Add(_moredekaiserFilter, c => c.Pool is Mordekaisercardpool);
        
        Dictionary<CharacterModel, NCardPoolFilter> _cardPoolFilters = (Dictionary<CharacterModel, NCardPoolFilter>)_cardPoolDictField!.GetValue(__instance)!;
        _cardPoolFilters.Add(ModelDb.Character<Characters.Mordekaiser>(), _moredekaiserFilter);

        Callable callable2 = Callable.From(delegate { _lastHoveredField?.SetValue(__instance, _moredekaiserFilter); });
        _moredekaiserFilter.Connect(Control.SignalName.FocusEntered,callable2);
        
        _moredekaiserFilter.TreeExiting += () =>
        {
            if (_moredekaiserFilter.IsConnected(NCardPoolFilter.SignalName.Toggled, callable1))
                _moredekaiserFilter.Disconnect(NCardPoolFilter.SignalName.Toggled, callable1);
            if (_moredekaiserFilter.IsConnected(Control.SignalName.FocusEntered, callable2))
                _moredekaiserFilter.Disconnect(Control.SignalName.FocusEntered, callable2);
        };
        
    }
    
}