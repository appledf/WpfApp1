# WpfApp1
## wpf控件 
###自定义进度条
![image](https://github.com/appledf/WpfApp1/assets/17972476/275028e9-3999-4600-b159-2695fe85a0f1)

 使用方法：
 
 `<uc:UCCycleProcessBar x:Name="uCCycleProcessBar" Value="10" Step="10" StartAngle="220" Width="500"></uc:UCCycleProcessBar>`
 
 名称|描述
 --|:--:
value|进度值
StartAngle|0值所在角度（以正上方为0角度）
width|为控件宽度
Step|步进幅度

### 仪表盘
![image](https://github.com/appledf/WpfApp1/assets/17972476/3e23ea5c-c611-4876-936c-588d818aeb57)

 
 使用方法
 
  `<uc:Instrument Grid.Row="1" Value="10" ></uc:Instrument> `
 
 名称|描述
 --|:--:
value|当前值
