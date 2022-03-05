import * as fs from 'fs'
import { opendir, readdir, readFile } from 'fs/promises'
import { join } from 'path'
const translateDir = '..\\Languages\\ChineseSimplified\\Keyed'
const diff = (set1: Set<string>, set2: Set<string>) => {
    return Array.from(set1).filter(k => !set2.has(k))
}
const readTag = (buffer: Buffer) => {
    const content = buffer.toString()
    const tagRegex = /<\/(\S+)>/g
    var tagSet = new Set<string>()
    while (true) {
        var res = tagRegex.exec(content)
        if (res !== null && res[1] !== 'LanguageData') {
            tagSet.add(res[1])
        }
        else
            break;
    }
    return tagSet
}
readdir('.').then(fileName => {
    const xmlFile = fileName.filter(f => f.endsWith('xml'))
    xmlFile.forEach(f => {
        const set1 = readTag(fs.readFileSync(f))
        const set2 = readTag(fs.readFileSync(join(translateDir, f)))
        const dif = diff(set1, set2)
        if (dif.length !== 0) console.log(`${f} require ${dif}`)

    })
})