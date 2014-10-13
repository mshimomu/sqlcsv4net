sqlcsv4net
==========

- まだお試し実装中
- CSVファイルに対してSQL(SELECT文)を発行できるコマンドラインツールです
- 裏ではSQLiteのインメモリモードを使用しています

## Usage

- 起動するとカレントディレクトリにあるCSVファイルを全て読み込みます
- CSVファイル名と同名、同じカラム名をもつテーブルが作成されます
    - CSVファイルの1行目はカラム名と見なされます
- SELECT文を発行すると結果が表示されます

```
C:\test\>sqlcsv
SQL*CSV: Release 0.0.0.0

now importing csv file(s)...
CREATE TABLE test(num,value) and import completed 0.0172349sec
SQL>select * from
SQL>test
SQL>;
1       AAA
2       あああ
3

3 row(s) selected
SQL>exit

C:\test\aaa>
```

- exit: SQL*CSVを終了
- quit: SQL*CSVを終了
