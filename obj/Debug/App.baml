   M S B A M L   `   `   `  ����G  CTimeforBreak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:TimeforBreakTimeforBreak  5          TimeforBreak.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ,localclr-namespace:TimeforBreak     5        �� 	Resources  5         ControlGreenColor  ControlRedColor  ControlGreenOverColor  ControlRedOverColor �� 
TargetType  GreenButton �� Property �� Value �� 
TargetType ��Name �� Color'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition	   GeneratedDuration	
   To   States   System.Windows.VisualState   
Storyboard �� TargetProperty8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value  	RedButton��%p  &	        &	 !     &	 B     &	 c     &	 �     7  &	 �    7  ��5   
   	#FF85c13F��65   ��5	   
   	#FFff5050��63   ��5
   
   	#FF90c752��69   ��5   
   	#FFff6666��67   �� 5   
   " ��6   .�5      ��  � �� �	#FF85c13F6+   �� 5       � ��$
 29px��6*   �� 5       � ��$ 103px��6*   �� 5       � �� �	#FFFFFFFF6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ Border��6U   #��z��6!   $��2q�6e   ���#00ace6��5      �� 5   "   # �   63    5        5   "   CommonStates�� 5   &     5   *   $	 0:0:0.5D�6;     5   *   $	 0D�6;   $
 Pressed��6Q   . 5    &     
Normal��62     5"   &   	MouseOver��. 5#   *   �� .��5$   .   �� $0 *(Panel.Background).(SolidColorBrush.Color)�6K   .g�5%   2     $ 0��6F   8   6R   5&   /   5'   +   5(   '   5*   #   ._�5,      �� $��2q�6/   $��Center=�5-      $��Center=�5.      ��. 5/      50      51      53      54      �� 56   
   " ��6   .�57      ��  � �� �	#FF85c13F6+   �� 58       � ��$
 29px��6*   �� 59       � ��$ 103px��6*   �� 5:       � �� �	#FFFFFFFF6+   �� 5;       � �� 5<      �� 5=      " ��6&   .R�5>      �� $ Border��6U   #��z��6!   $��1q�6e   ���#8c8c8c��5?      �� 5@   "   # �  63    5B        5C   "   CommonStates�� 5D   &     5E   *   $	 0:0:0.5D�6;     5F   *   $	 0D�6;   $
 Pressed��6Q   . 5H   &     
Normal��62     5J   &   	MouseOver��. 5K   *   �� .��5L   .   �� $0 *(Panel.Background).(SolidColorBrush.Color)�6K   .g�5M   2     $ 0��6F   8   6R   5N   /   5O   +   5P   '   5R   #   ._�5T      �� $��2q�6/   $��Center=�5U      $��Center=�5V      ��. 5W      5X      5Y      5[      5]      5`      5a      