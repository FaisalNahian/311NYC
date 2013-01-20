﻿<phone:PhoneApplicationPage
    x:Class="$rootnamespace$.Pages.LoginPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone"
    xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:fed="clr-namespace:Microsoft.WindowsAzure.Samples.Phone.Identity.AccessControl;assembly=Phone.Identity.AccessControl"
    FontFamily="{StaticResource PhoneFontFamilyNormal}"
    FontSize="{StaticResource PhoneFontSizeNormal}"
    Foreground="{StaticResource PhoneForegroundBrush}"
    SupportedOrientations="PortraitOrLandscape" Orientation="Portrait"
    mc:Ignorable="d" d:DesignHeight="768" d:DesignWidth="480"
    shell:SystemTray.IsVisible="True">

  <phone:PhoneApplicationPage.Resources>
    <!--Page Transitions-->
    <Storyboard x:Name="PageTransitionReset">
      <DoubleAnimationUsingKeyFrames Storyboard.TargetName="LayoutRoot" Storyboard.TargetProperty="(UIElement.Projection).(PlaneProjection.RotationY)">
        <EasingDoubleKeyFrame KeyTime="00:00:00" Value="90"/>
      </DoubleAnimationUsingKeyFrames>
      <DoubleAnimationUsingKeyFrames Storyboard.TargetName="LayoutRoot" Storyboard.TargetProperty="(UIElement.Projection).(PlaneProjection.CenterOfRotationX)">
        <EasingDoubleKeyFrame KeyTime="00:00:00" Value="0"/>
      </DoubleAnimationUsingKeyFrames>
    </Storyboard>
    <Storyboard x:Name="PageTransitionIn">
      <DoubleAnimationUsingKeyFrames Storyboard.TargetName="LayoutRoot" Storyboard.TargetProperty="(UIElement.Projection).(PlaneProjection.RotationY)">
        <EasingDoubleKeyFrame KeyTime="00:00:00" Value="90"/>
        <EasingDoubleKeyFrame KeyTime="00:00:00.3" Value="0">
          <EasingDoubleKeyFrame.EasingFunction>
            <CircleEase EasingMode="EaseIn"/>
          </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
      </DoubleAnimationUsingKeyFrames>
      <DoubleAnimationUsingKeyFrames Storyboard.TargetName="LayoutRoot" Storyboard.TargetProperty="(UIElement.Projection).(PlaneProjection.CenterOfRotationX)">
        <EasingDoubleKeyFrame KeyTime="00:00:00" Value="0"/>
        <EasingDoubleKeyFrame KeyTime="00:00:00.3" Value="0">
          <EasingDoubleKeyFrame.EasingFunction>
            <CircleEase EasingMode="EaseIn"/>
          </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
      </DoubleAnimationUsingKeyFrames>
    </Storyboard>
  </phone:PhoneApplicationPage.Resources>

  <!--LayoutRoot contains the root grid where all other page content is placed-->
  <Grid x:Name="LayoutRoot" Background="Transparent">
    <Grid.Projection>
      <PlaneProjection />
    </Grid.Projection>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto"/>
      <RowDefinition Height="*"/>
    </Grid.RowDefinitions>

    <!--TitlePanel contains the name of the application and page title-->
    <StackPanel Grid.Row="0" Margin="12,17,0,28">
      <TextBlock Text="$rootnamespace$" Style="{StaticResource PhoneTextNormalStyle}"/>
      <TextBlock Text="log in" Margin="9,-7,0,0" Style="{StaticResource PhoneTextTitle1Style}"/>
    </StackPanel>

    <!--ContentPanel - place additional content here-->
    <Grid Grid.Row="1" Margin="12,12,0,12">
      <fed:AccessControlServiceSignIn
          x:Name="SignInControl"
          Realm="{StaticResource realm}"
          ServiceNamespace="{StaticResource acsNamespace}"
          SimpleWebTokenStore="{StaticResource swtStore}" />
    </Grid>
  </Grid>

</phone:PhoneApplicationPage>