   M S B A M L   `   `   `  ����   todolistmanagercsharp0    *todolistmanagercsharp.Views.EditTaskWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  	Edit Task��$	��450��$	��800�� � WindowStartupLocation$ CenterScreen=�   HoverBackground#���   .��� $��20q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��  SidebarBackground#���   �� Padding$ 10q� �� CornerRadius$ 5��._��� $��	Edit Task��  ButtonForeground#���  $��Center=�$��16(�$
��Bold"�� $��1��$��	0,20,0,20q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* � � ColumnDefinitions �� $
��Auto ��� $��* �.+��� $��0��$��0��$��Task Title:��#���  $��0,0,10,5q�$��Center=��� $��0��$��1��$��0,0,0,5q����� *Title+ �� UpdateSourceTrigger$ PropertyChanged=��� $��1��$��0��$��	Priority:��#���  $��0,0,10,5q�$��Center=��� $��1��$��1��$��0,0,0,5q� �� SelectedItem �� *TaskPriority+$ PropertyChanged=���O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem �� 6sys&clr-namespace:System;assembly=mscorlib      �� Type" ��.s���Low����
Medium����High���� $��2��$��0��$��Status:��#���  $��0,0,10,5q�$��Center=��� $��2��$��1��$��0,0,0,5q� �� *	TaskState+$ PropertyChanged=���1&clr-namespace:System;assembly=mscorlibSystem �� 6sys&clr-namespace:System;assembly=mscorlib     " ��.s���Not Started����In Progress����	Completed���� $��3��$��0��$��	Due Date:��#���  $��0,0,10,5q�$��Center=�(  "System.Windows.Controls.DatePicker  $��3��$��1��$��0,0,0,5q�	   SelectedDate	 �� *	Duedate+$ PropertyChanged=��� $��4��$��0��$��Recurrence:��#���  $��0,0,10,5q�$��Center=��� $��4��$��1��$��0,0,0,5q� �� *
Recurrence+$ PropertyChanged=���1&clr-namespace:System;assembly=mscorlibSystem �� 6sys&clr-namespace:System;assembly=mscorlib     " ��.s���None����	Daily����
Weekly����Monthly���� $��5��$��0��$��Description:��#���  $��0,0,10,5q�$	��Top=��� $��5��$��1��
 �� TextWrapping$

 Wrap=� �� AcceptsReturn . $	��100��" �� VerticalScrollBarVisibility$
 Auto=�$��0,0,0,5q����� *Description+$ PropertyChanged=��� $��2�� �� Orientation$ 
Horizontal=�$��Right=�$��0,10,0,0q�.+��� $��Cancel��$	��5,0q����� *CloseWindowCommand+�� $��Save Changes��$	��5,0q����� *SaveEditedTaskCommand+