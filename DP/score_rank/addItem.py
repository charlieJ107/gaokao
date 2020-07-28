import csv

itemTable_read = open("./score_rank_item.csv", 'r', encoding='utf-8')
finalLine=itemTable_read.readlines()[-1].split(',')
finalId = int(finalLine[0]) + 1
finalTableId=int(finalLine[1])+1
itemTable_read.close()

itemTable_write = open(".\\score_rank_item.csv", 'a', encoding='utf-8')
wait2add = open("./wait_to_add.csv", 'r', encoding='utf-8')
for i in wait2add.readlines():
    newAdd = str(finalId) + ',' +str(finalTableId)+','+ i.replace("﻿", "")
    itemTable_write.write(newAdd)
    print(newAdd)
    finalId+=1






itemTable_write.close()
wait2add.close()
