   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:WpfApplication1.ModelWpfApplication1.Model  5      E'clr-namespace:WpfApplication1.ViewModelWpfApplication1.ViewModel  5	      ;"clr-namespace:WpfApplication1.ViewWpfApplication1.View  5
      /    )WpfApplication1.View.TabsFolderPlanesView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3mdl#clr-namespace:WpfApplication1.Model     5      6vm'clr-namespace:WpfApplication1.ViewModel     5	      1vw"clr-namespace:WpfApplication1.View     5
        � 	Resources  5         FolderPlaneHeader �� DataType'   !WpfApplication1.Model.FolderPlane � ToolTip   ButtonOnlyScrollViewerDataVm �� 
TargetType � IsSharedSizeScope � ColumnDefinitions z� SharedSizeGroup �� CanContentScroll  RoundButton  
FolderTabs, �� %ComputedHorizontalScrollBarVisibility  ButtonOnlyScrollViewer	 ��Name  FolderPlaneTabs
 �� 
TargetType �� Property �� Value �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName  FolderTabsSelected  FolderTabsNotSelected��%�  &	        &	 �     7  7  7  7  &	 D    7  &	 �    7  7  �� 5      "  6-   .R�5   
   �� $��10,4,8,4q�6   $��10(�6&   
����  ���5      �� *FriendlyName+ 62   �� *FullPathName+5      �� 5'      " ��6;   .R�5(      �  .  5)   
   �� 5*      $��* �6   �� 5+      $
��Auto �6   $ ScrollButtonW��6*   .+�50   
   �� $��0��51      #��z��52      #��z��53      #��z��54      # z��55      #��z��56      �� 57   
   $��1��6   $��3q�6%   .+�58      �� $ Scroll Tabs Left��59      8��  8�� #y�z 5<      $��ScrollBar.PageLeftCommand��5=      $	����5>      $��Marlett)�5?      $��14(�5@      �� 5A      $ Scroll Tabs Right��5B      8�� 8�� #y�z 5E      $ ��ScrollBar.PageRightCommand��5F      $	����5G      $��Marlett)�5H      $��14(�5I      5J      5L   	   5M      �� 5R      " ��65   .R�5S      �� $	 ScrollViewer��6   8��  .��5T   
   �� 5U   	   5V      �� 5X      "
 ��6$   .�5Y      ��  � �� 5Z   
   �� 5[      " ��6   .R�5\      �� $��ListboxItemBorder��6   $ 	10,10,0,0��6.   $��3,3,3,0q�6G   ._�5]      �� 5^       5_      P� 5`       �@��
IsSelected . . �5a      �� $ ListboxItemBorder��6   8    � ��6N   5b      P� 5c       �@��
IsSelected .  . �5d      �� $ ListboxItemBorder��6   8   � ��6N   5e      5g      5i   	   5�      5�      .��5�      �  5�      �� 5�      $
��Auto �6   �� 5�      $��* �6   �� 5�      $
��Auto �6   .+�5�      �� $��0��6   $��3q�6!   .+�5�      �� -   5         ��Name$ CloseCurrentTab��5�   	   #��[ 5�   	   $ 关闭当前标签/路径��5�   	   $	����5�   	   $��WebDings)�5�   	   
���Gc����5�   	   �� *CloseTabCommand+�� 5�      -   5        $ FolderUp��5�   	   #��[ 5�   	   $ FolderUp��5�   	   $	����5�   	   $��	WingDings)�5�   	   
��� �����5�   	   �� *FolderUpCommand+5�      �� 5�      -   5         ��Name$ FolderPlanesHeaders��5�      $��1��5�      #��[  5�      $ �� IsSynchronizedWithCurrentItem$
 TrueY�5�      #��[ 5�      #��[ 5�      ,   &WpfApplication1.View.DnDSortOneListBox   Attach1 . ��5�      �� *FolderPlanes+ �� SelectedItem 5�      �� *SelectedFolderPlane+ �� Mode$ TwoWay=���5�      �� 5�   
   .R�5�      ��  �� Orientation$ 
Horizontal=�6   5�      5�      �� 5�      $��20��6   $��26��6   $��2��6$   |�.  $ List tabs and select��5�      .��5�      o� $��20��6   #��[ 6   .@�5�   
   �� $��-6,-2,-6,-2q�5�       �� DisplayMemberPath$ FullPathName��5�      $
 TrueY�5�      ��5�      �� *FolderPlanes+ 5�      ��  �� Path$ SelectedFolderPlane�$ TwoWay=�5�   	   5�      5�      5�      