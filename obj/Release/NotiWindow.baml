   M S B A M L   `   `   `  ���� G  CTimeforBreak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:TimeforBreakTimeforBreak      TimeforBreak.NotiWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:TimeforBreak       � Title$  	TimeBreak��$	��300��$	��450�� � WindowStyle$
 None=� � AllowsTransparency .  J� Opacity$
 0.95Q�
������  � WindowStartupLocation$ Manual=� � Left$ 0��
 � Top$ 0��.���� -   $	��brd�� �� CornerRadius$ 15�����#f9f9ff._��  J� RenderTransformOrigin$	 0,1&�.+��� -   	 ��Name$	 label��$��Time for Break Reminder��$
��Left=�$��	95,31,0,0q�$	��Top=�$��20(�$
��Bold"��� -   $	 label1��$"��Hey! You've been working for��$
��Left=�$��35��$��	33,76,0,0q�$	��Top=�$	��404��$��16(��� -   $	 label1_copy��$��	How about��$
��Left=�$��35��$��
330,76,0,0q�$	��Top=�$	��107��$��16(�	� -   
 	�Name$
 radioButton��$��Center=�$��Yes, I'd like to.��$
��Left=�$��26��$��
49,168,0,0q�$	��Top=�$	��287��$��16(��� -   $	 label1_Copy��$��standing up��$
��Left=�$��35��$��	33,98,0,0q�$	��Top=�$	��110��$��16(��� -   $	 label2_copy��$��30 minutes.�����	#FF468499$
��Left=�$��27��$��
244,77,0,0q�$	��Top=�$	��105��$��16(��� -   $	 label1_Copy1��$��or taking a walk?��$
��Left=�$��35��$��
123,98,0,0q�$	��Top=�$	��266��$��16(��� -	   $	 label5��
��� ���$�� ��$
��Left=�$��44��$��
33,246,0,0q�$	��Top=�$	��391��	� -
   $
 radioButton1��$��Center=�$��No, thanks. ��$
��Left=�$��26��$��
49,210,0,0q�$	��Top=�$	��287��$��16(��� -    ��Name$ timer��$��	timer.ico��$��27,16,362,224q�$
��Fill=� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� .���� .��U�  �� TargetProperty$9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�.\���  O� KeyTime$ 0:0:0�� O� Value$ 0Q��� $ 0:0:0.5��$ 1Q�U� $ (UIElement.Opacity)�.\��� $ 0:0:2��$ 1Q��� $ 0:0:4��$ 1Q�z���  �� ScaleY$ 1Q�