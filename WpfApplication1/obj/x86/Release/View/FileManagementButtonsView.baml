   M S B A M L   `   `   `  ����   WpfApplication14    .WpfApplication1.View.FileManagementButtonsView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $��52.442��$��556.809��.����   �� Orientation$  
Horizontal=� � 	Resources    Bool2Vis��%   &	        �� .+��� -    ��Name$ CheckFM��$��File Management��$��Center=� � ToolTip$2 ,At own Risk, not fully tested and functional�� d� 	IsChecked$
 TrueY��� y��� *	IsChecked+ �� ElementName$ CheckFM�� �� 	Converter# [  ���� .R��� $  
Horizontal=�.@��� #��["��� -    ��Name$ 	NewFolder��  RoundButton#��[ $ 新增目录�� ^� ShowOnDisabled . $��新增目录��
��� ���$��1,8,1,10q�$
��Auto��$��55������ *NewFolderCommand+�� #��["��� -   $ DeleteSelected��$��
10,10,10,8q�#��[ $ 删除当前选中项�� . $��删除��
���  ��$
��Auto��$��55������ *DeleteSelectedCommand+� $��Slide=�	 � 	StaysOpen	 .  $��Right=����� *Popup3IsOpen+.���� 
���   �$��2q�._��� 
�������.+��� $'��!Rename Current Selected filename:��$��10q�$��10q�.���� $  
Horizontal=�.+��� -   $��SaveItem��$��Stretch=�$	��200������ *SelectedFolderItem+�� $��Rename������ *RenameCommand+���� 
 �� Path$

 Text�$ SaveItem���� $��Cancel��$��5q����� *ToggleOpenPopup3Command+�� #��["��� -   $ DeleteSelected1��$��	10,9,10,6q�#��[ $ 编辑模型�� . $��编辑模型��
���`���$
��Auto��$��55������ *	EditModel+�� #��["��� -   $ Refresh��#��[ $ 
刷新(F5)�� . $��刷新��
����ح�$
��Auto��$��1,10,1,8q�$��55������ *RefreshFolderPlanesCommand+�� #��["��� -   $	 Btn��$��导入模型��$��55��$��0,11,0,8q�#��[ $
��Auto��
���/������� *ImPortModelCommand+�� #��["��� -   $ BtnPi��$��批量生成xml��$��74��$��0,11,0,8q�#��[ $
��Auto��
����������� *NumbersImportCommand+