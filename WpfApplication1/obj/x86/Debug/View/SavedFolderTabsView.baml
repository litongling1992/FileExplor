   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:WpfApplication1.ModelWpfApplication1.Model  5      ;"clr-namespace:WpfApplication1.ViewWpfApplication1.View  5      .    (WpfApplication1.View.SavedFolderTabsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3mdl#clr-namespace:WpfApplication1.Model     5      7dragdrop"clr-namespace:WpfApplication1.View     5        � 	Resources  5         templateSavedFolderTabs �� DataType/   )WpfApplication1.Model.SavedFolderTabsItem �� Path��%�   &	        �� 5      "  63   .R�5   
   �� $��10(�66   $��0,0,1,0q�6D   
���������6   �� $ FriendlyName�6^   5      5      .��5      �� -   5         ��Name$ SavedFolderTabsUc��5       �� Orientation$ 
Horizontal=�6*   .+�5      ��  � ToolTip$4 .Open several saved tabs by click on one button��5        selectedItemUseBrusch#��[ 5      #��[  5      ,   &WpfApplication1.View.DnDSortOneListBox   Attach1 . ��5      �� $ SavedFolderTabs� �� SelectedIndex 5      �� *SelectedIndexSavedFolderTabs+��5      �� 5   
   .R�5      �� $ 
Horizontal=�6   5      5       �� 5"      |�.  
�������$ Close all current Tabs��5&      ��5%      �� *CloseAllFolderTabsCommand+.��5'      �� .+�5(   
   � $��20��6   .+�5)      A� 
���Gc��$��20��5+      $��20��5,      $��Center=�5-      $��Center=�5.      �� 5/      $	����50      $��20(�51      $��0q�52      $��WebDings)�53      
���   �$
��Bold"�55      $��Center=�56      57      �� 58   
   $��	Close All��6   $��10(�6$   $��2q�62   59   	   5:      �� 5<      |�.  
�������$( "Delete one named Saved Tabs Button��5@      ��5?      �� *ToggleOpenPopup1Command+.��5A      �� .+�5B   
   �� $	����5C      $��20(�5D      $��20��5E      $��0q�5F      $��WebDings)�5G      
���  ��$
��Bold"�5I      $��Center=�5J      �� 5K   
   $��Saved��6   $��10(�6    $��2q�6.   5L   	   5M      � 5O      $��Slide=�5Q       � 	StaysOpen .  $��Right=�5S      ��5P      �� *Popup1IsOpen+.��5T      �� 
���   �$��2q�6#   ._�5U   
   �� 
�������.+�5V      �� $2��,Delete name from list for saved Folder Tabs ��6   $��10q�6K   $��10q�6W   .��5W      �� .+�5X      �� -   5        $��Listbox1��5X      #��[  6P   ��6(   �� *SavedFolderTabs+6P   �� 5Y      $��Delete Selected��6   ��61   �� *DeleteSavedTabsCommand+��6\   �� $ SelectedIndex�	 �� ElementName$	 Listbox1��5Z      5[      �� 5\      $��Cancel��6   $��5q�6P   ��6$   �� *ToggleOpenPopup1Command+6P   5]      5^   	   5_      �� 5a      |�.  
�������$3 -Make a named Button to store all current tabs��5e      ��5d      �� *ToggleOpenPopup2Command+.��5f      �� .+�5g   
   �� $2��,/WpfApplication1;component/MyImages/save.ico��6   $��20��6F   �� 5h   
   $��All Tabs��6   $��10(�6#   $��2q�61   5i   	   5j      � 5l      $��Slide=�5n       .  $��Right=�5p      ��5m      �� *Popup2IsOpen+.��5q      �� 
���   �$��2q�6#   ._�5r   
   �� 
�������.+�5t      �� $4��.Add a new name to save all current FolderTabs:��6   $��10q�6M   $��10q�6Y   .��5u      �� $ 
Horizontal=�6   .+�5v      �� -   5        $��SaveItem��5w      $��Enter here..��5x      $��Stretch=�5y      $	��200��5z      �� 5{      $	��Add��6   ��6%   �� *AddSavedTabsCommand+��6M   �� $
 Text�$	 SaveItem��5|      5}      �� 5~      $��Cancel��6   $��5q�6P   ��6$   �� *ToggleOpenPopup2Command+6P   5      5�   	   5�      5�      5�      