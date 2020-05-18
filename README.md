# pp-server



### 接口

```
GET /pp
```

### 参数

```
bid					BeatmapId
mode				游戏模式 0 = osu!,1 = taiko, 3 = mania(暂不支持ctb)
mods				模式直接使用简称即可 例如 HDDT，DTHR，HRHD
acc					acc百分比0-100
miss				miss个数
combo				combo个数
good				100的个数
meh					50的个数
score				成绩分数
percentCombo		设置一个百分比，自动计算连击数，combo = percentCombo * maxcombo
```



### 示例

![image-20200518142328659](https://raw.githubusercontent.com/wanjiaXG/pp-server/master/image/image-20200518142328659.png)