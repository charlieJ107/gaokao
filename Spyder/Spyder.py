import requests
from bs4 import BeautifulSoup
import csv

header={

	# TODO: 填上你的Cookie
	"Cookie": "Cookie",
	"Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
	"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:78.0) Gecko/20100101 Firefox/78.0"
	}

urlbase="https://gaokao.chsi.com.cn/gkxx/zc/ss/201906/20190623/1799991544-"
url_list=[]
for i in range(4):
	url=urlbase+str(i)+".html"
	url_list.append(url)

session=requests.Session()

def resolve2list(text):
	bsObj=BeautifulSoup(text)
	table=bsObj.find("table",{"class":"table01"})
	page_table_list=list()
	for rows in table.findAll("tr"): # 表的每行
		rowList=list()
		for items in rows.children:
			rowList.append(items.get_text())
		print(rowList)
		page_table_list.append(rowList)
		
	
#要不要写数据库?
f=open("./广西-2019-一分一段表.csv", "w")
writer = csv.writer(f, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)
for url in url_list:
	res=session.get(url, headers=header)
	table=resolve2list(res.text)
	print(table)
	

f.close()

