import urllib.request
import random
import time

userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36"

armorFile = open("armorUrl.txt", "r")

for line in armorFile:
    if("<a href" in line):
        urlEnd = line.index('>') - 1
        url = line[9:urlEnd]
        print(url)

        startArmorName = line.rfind("/") + 1
        outfileName = line[startArmorName:urlEnd]
        outfileName = "armor/" + outfileName + ".html"
        print(outfileName)

        req = urllib.request.Request(url, headers={'User-Agent' : userAgent}) 
        con = urllib.request.urlopen( req )

        outFile = open(outfileName, "w", encoding = 'utf-8')
        outFile.write(con.read().decode('utf-8'))

        outFile.close();

        sleepLength = random.randint(5, 25) + (random.randint(0, 999)/1000)
        print("Sleeping for " + str(sleepLength) + " seconds. ")
        time.sleep(sleepLength)

armorFile.close()
print("Done")

