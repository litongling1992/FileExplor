   M S B A M L   `   `   `  ����  WpfApplication14    .WpfApplication1.View.FileManagementButtonsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $��52.442��5      $��556.809��6$   .��5	      ��   �� Orientation$  
Horizontal=�6    � 	Resources 5
   
      Bool2Vis��%   &	        �� 5      6+   5      .+�5   
   �� -   5         ��Name$ CheckFM��5      $��File Management��5      $��Center=�5       � ToolTip$2 ,At own Risk, not fully tested and functional��5       d� 	IsChecked$
 TrueY�5      �� 5   
   y�6   �� *	IsChecked+ �� ElementName$ CheckFM�� �� 	Converter# [  ��5      �� 5      .R�5      �� $  
Horizontal=�6!   5      .@�5      �� #��["�6   �� 5N      -   5         ��Name$ 	NewFolder��5O   	     RoundButton#��[ 5P   	   $ 新增目录��5Q   	    ^� ShowOnDisabled . $��新增目录��5S   	   
��� ���$��1,8,1,10q�5V   /   $
��Auto��6A   $��55��6O   ��6	   �� *NewFolderCommand+6O   �� 5Y      #��["�6   �� 5f      -   5        $ DeleteSelected��5g      $��
10,10,10,8q�6)   #��[ 5h      $ 删除当前选中项��5i       . $��删除��5k      
���  ��$
��Auto��5n   ;   $��55��6I   ��6   �� *DeleteSelectedCommand+6I   � 5p      $��Slide=�5r      	 � 	StaysOpen	 .  $��Right=�5t      ��5q      �� *Popup3IsOpen+.��5u      �� 
���   �$��2q�6-   ._�5v      �� 
�������.+�5x      �� $'��!Rename Current Selected filename:��6#   $��10q�6N   $��10q�6Z   .��5y      �� $  
Horizontal=�6)   .+�5z   "   �� -   5        $��SaveItem��5{      $��Stretch=�5}      $	��200��5~      ��5|      �� *SelectedFolderItem+5~      �� 5   "   $��Rename��6)   ��6:   �� *RenameCommand+��6\   �� 
 �� Path$

 Text�$ SaveItem��5�      5�      �� 5�      $��Cancel��6!   $��5q�6^   ��62   �� *ToggleOpenPopup3Command+6^   5�      5�      5�      �� 5�      #��["�6   �� 5�      -   5        $ DeleteSelected1��5�      $��	10,9,10,6q�6*   #��[ 5�      $ 编辑模型��5�       . $��编辑模型��5�      
���`���$
��Auto��5�   /   $��55��6=   ��6   �� *	EditModel+6=   �� 5�      #��["�6   5�      �� 5�   
   -   5        $ Refresh��5�      #��[ 5�      $ 
刷新(F5)��5�       . $��刷新��5�      
����ح�$
��Auto��5�   ;   $��1,10,1,8q�6I   $��55��6[   ��6   �� *RefreshFolderPlanesCommand+6[   �� 5�   
   #��["�6   �� 5�   
   -   5        $	 Btn��5�      $��导入模型��6   $��55��6T   $��0,11,0,8q�6_   #��[ 5�      $
��Auto��61   
���/�����5�   -   �� *ImPortModelCommand+5�      �� 5�   
   #��["�6   �� 5�   
   -   5        $ BtnPi��5�      $��批量生成xml��6    $��74��6[   $��0,11,0,8q�6f   #��[ 5�      $
��Auto��61   
���������5�   2   �� *NumbersImportCommand+5�      5�      5�      