   M S B A M L   `   `   `  ���� J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:WpfApplication1.ViewWpfApplication1.View  '    !WpfApplication1.View.ImportWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:WpfApplication1.View       � Title$  导入模型��$��655.8��$	��604�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=�.��� ���	#FFDEDDE4$��0,0,2,3q� � ColumnDefinitions �� $
��274* ��� $
��333* �.+��� -    ��Name$ label��$��模型源路径��$
��Left=�$��31��$��	15,57,0,0q�$	��Top=�$��72���� -    ��Name$ TxtSourcePath��$��39��$��
87,49,97,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��2���� -   $ 
label_Copy��$��模型类型��$
��Left=�$��31��$��
26,106,0,0q�$	��Top=�$��72��	� -    	�Name$ RbFurniture��$��家具��$��24��$��103,113,0,0q�$	��Top=�$
��Left=�$��60���� -   $ label_Copy1��$��模型名称��$
��Left=�$��31��$��
22,161,0,0q�$	��Top=�$��72���� -   $ label_Copy2��$��中文��$
��Left=�$��31��$��
74,142,0,0q�$	��Top=�$��50���� -   $ label_Copy3��$��英文��$
��Left=�$��
74,178,0,0q�$��50��$��31��$	��Top=��� -   $ TxtModelChineseName��$��31��$��112,142,95,0q�$
 Wrap=�$	��Top=�$��2���� -	   $ TxtModelEnglishName��$��112,178,95,0q�$
 Wrap=�$��31��$	��Top=�$��2��	� -
   $ 
RbMaterial��$��材质��$��
103,93,0,0q�$��24��$	��Top=�$
��Left=�$��72��� -    �Name$ 
GdProperty��$��10,234,10,35q�$��2��.+��� 
�������$��1q�$��-10,-13,10,11q�._��� -   	 ��Name$	 GbDisFromFloor��$��家具默认离地高度��$��68��$	��Top=�$
��Left=�$	��250��$��264,267,0,0q�.��� $��0,0,18,0q� �� $��8* ��� $��9* �.+��� -   $ TxtDefDisFromFloor��$��31��$��	0,10,71,0q�$
 Wrap=�$	��Top=�$��2���� -   $ label_Copy4��$��(单位：mm)��$
��Left=�$��31��$��45,10,-22,0q�$	��Top=�$��93��$��1���� -   $ labelProperty��$��家具模型属性设置��$
��Left=�$��10,0,0,373.963q�$��Bottom=��� -   $	 GbTransProperty��$��家具透明属性��$��	47,0,0,39q�$��66��$��Bottom=�$
��Left=�$	��160��.���  �� $	��71* ��� $	��77* �.+�	� -   $ RbTransparent��$��透明��$��10,6,8,0q�$	��Top=�	� -   $ RbTranslucent��$��0��$��	不透明��$��	10,26,8,0q�$	��Top=��� -   $	 GbFurVerticalProperty��$��家具垂直属性��$��47,82,284,120q�.��� $��0,0,13,0q�$	��120��$��Bottom=�.+�	� -   $ 	RbInFloor��$��嵌入地板��$��16��$��	16,0,0,97q�$��Bottom=�$
��Left=�$��66��	� -   $ RbFloorMove��$��可自由移动��$
��Left=�$��22��$��	16,76,0,0q�$	��Top=�$��81��	� -   $ 	RbOnFloor��$��在地板上��$��21��$��	16,0,0,71q�$��Bottom=�$
��Left=�$��70��	� -   $ RbOnCelling��$��贴在天花板上��$
��Left=�$��19��$��	16,0,0,47q�$��Bottom=�$	��101��	� -   $ RbInCelling��$��嵌入天花板��$
��Left=�$��25��$��	16,95,0,0q�$	��Top=�$��92���� -   $	 GbFurWallProperty��$��家具靠墙属性��$��47,0,0,0q�$��77��$	��Top=�$
��Left=�$	��149��.��� $��0,0,17,-2.837q�
 J� RenderTransformOrigin$
 0.575,0.631&�$��92.94��$	��Top=� �� $	��57* ��� $	��45* ��� $
��113* �.+�	� -   $ RbInWall��$��嵌入墙体��$��7.469,10,0,64.126q�$��3��$
��Left=�$��103.031��	� -   $ RbOnWall��$��可以贴墙��$��7.469,0,-28.802,33.293q�$��Bottom=�$��25.833��$
��Left=�$
��78.5��$��2��$��0���� -    ��Name$ 	BtnImport��$��导入��$��35��$��Bottom=�$��205,0,211,4q�$��2���� -   $ 	BtnSearch��$��浏览��$��31��$��Bottom=�$��235,0,31,543q�$
 0.418,0.484&�$��1��