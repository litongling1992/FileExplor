   M S B A M L   `   `   `  ���� J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:WpfApplication1.ModelWpfApplication1.Model  ;"clr-namespace:WpfApplication1.ViewWpfApplication1.View  .    (WpfApplication1.View.SavedFolderTabsView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3mdl#clr-namespace:WpfApplication1.Model     7dragdrop"clr-namespace:WpfApplication1.View       � 	Resources     templateSavedFolderTabs �� DataType/   )WpfApplication1.Model.SavedFolderTabsItem �� Path��%Z   &	        �� "  .R��� $��10(�$��0,0,1,0q�
����������� $ FriendlyName�.���� -    ��Name$ SavedFolderTabsUc�� �� Orientation$ 
Horizontal=�.+���  � ToolTip$4 .Open several saved tabs by click on one button��  selectedItemUseBrusch#��[ #��[  ,   &WpfApplication1.View.DnDSortOneListBox   Attach1 . ���� $ SavedFolderTabs� �� SelectedIndex �� *SelectedIndexSavedFolderTabs+���� .R��� $ 
Horizontal=��� |�.  
�������$ Close all current Tabs������ *CloseAllFolderTabsCommand+.���� .+�� $��20��.+�A� 
���Gc��$��20��$��20��$��Center=�$��Center=��� $	����$��20(�$��0q�$��WebDings)�
���   �$
��Bold"�$��Center=��� $��	Close All��$��10(�$��2q��� |�.  
�������$( "Delete one named Saved Tabs Button������ *ToggleOpenPopup1Command+.���� .+��� $	����$��20(�$��20��$��0q�$��WebDings)�
���  ��$
��Bold"�$��Center=��� $��Saved��$��10(�$��2q�� $��Slide=� � 	StaysOpen .  $��Right=����� *Popup1IsOpen+.���� 
���   �$��2q�._��� 
�������.+��� $2��,Delete name from list for saved Folder Tabs ��$��10q�$��10q�.���� .+��� -   $��Listbox1��#��[  ���� *SavedFolderTabs+�� $��Delete Selected������ *DeleteSavedTabsCommand+���� $ SelectedIndex�	 �� ElementName$	 Listbox1���� $��Cancel��$��5q����� *ToggleOpenPopup1Command+�� |�.  
�������$3 -Make a named Button to store all current tabs������ *ToggleOpenPopup2Command+.���� .+��� $2��,/WpfApplication1;component/MyImages/save.ico��$��20���� $��All Tabs��$��10(�$��2q�� $��Slide=� .  $��Right=����� *Popup2IsOpen+.���� 
���   �$��2q�._��� 
�������.+��� $4��.Add a new name to save all current FolderTabs:��$��10q�$��10q�.���� $ 
Horizontal=�.+��� -   $��SaveItem��$��Enter here..��$��Stretch=�$	��200���� $	��Add������ *AddSavedTabsCommand+���� $
 Text�$	 SaveItem���� $��Cancel��$��5q����� *ToggleOpenPopup2Command+