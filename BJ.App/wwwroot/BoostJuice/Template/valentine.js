
class AnimateHeartCanvas {
    /**
     * @param hMin màu h phút
     * @param hMax màu h tối đa
     * @param countHeart số lượng trái tim
     * @param sizeMin hình trái tim tối thiểu
     * @param sizeMax hình trái tim tối đa
     * @param bgColor màu nền
     */
    constructor(hMin, hMax, countHeart = 20, sizeMin = 50, sizeMax = 350, bgColor) {
        this.isPlaying = true // Chơi tự động theo mặc định

        this.mouseX = 0
        this.mouseY = 0

        this.configFrame = {
            width: 1200,
            height: 300,
            bgColor: bgColor
        }
        this.configHeart = {
            timeLine: 0,                           // mốc thời gian

            timeInit: new Date().getTime(),
            movement: 0.5,                           // Tôc độ di chuyển

            x: 50,                                 // Vị trí x
            y: 50,                                 // Vị trí y
            width: 200,                            // heart kích cỡ
            height: 200,                           // heart kích cỡ
            countHeart: countHeart || 20,         // heart Số lượng
            // kích cỡ
            sizeMin: isNaN(sizeMin) ? 50 : sizeMin,  // giá trị tối thiểu
            sizeMax: isNaN(sizeMax) ? 350 : sizeMax, // giá trị tối đa

            // màu sắc

            colorSaturate: 100,                    // độ bão hòa màu 0-100
            colorLight: 60,                        // Độ sáng màu 0-100
            hMin: isNaN(hMin) ? 330 : hMin,          // Giá trị màu tối thiểu
            hMax: isNaN(hMax) ? 350 : hMax,          // Giá trị màu tối đa
            minOpacity: 20,                       
            maxOpacity: 100,                       
            opacityGrowth: 5,                      

            // Phạm vi vị trí xuất hiện
            heartRangeMin: 0,                      
            heartRangeMax: 0.3,

            // sự tăng tốc
            gravityMin: 1,                         // sự tăng tốc min
            gravityMax: 9.8,                       // sự tăng tốc max

            flowDirection: 1,                      // Hướng di chuyển 1 hướng lên -1 xuống

        }

        this.heartBuffer = [] 

        this.init()

        window.onresize = () => {
            this.configFrame.height = innerHeight * 2
            this.configFrame.width = innerWidth * 2
            let heartLayer = document.getElementById('heartLayer')
            this.updateFrameAttribute(heartLayer)
        }
    }

    play() {
        if (this.isPlaying) {

        } else {
            this.isPlaying = true
            this.draw()
        }
        this.preventDefault();
    }
    stop() {
        this.isPlaying = false
    }

    moveDown() {
        this.configHeart.flowDirection = -1
    }
    moveUp() {
        this.configHeart.flowDirection = 1
    }

    speedUp() { }
    speedDown() { }

    destroy() {
        this.isPlaying = false
        let heartLayer = document.getElementById('heartLayer')
        heartLayer.remove()
        console.log('Stop')
    }

    updateFrameAttribute(heartLayer) {
        heartLayer.setAttribute('id', 'heartLayer')
        heartLayer.setAttribute('width', this.configFrame.width)
        heartLayer.setAttribute('height', this.configFrame.height)
        heartLayer.style.width = `${this.configFrame.width / 2}px`
        heartLayer.style.height = `${this.configFrame.height / 2}px`
        heartLayer.style.zIndex = '-1'
        heartLayer.style.userSelect = 'none'
        heartLayer.style.position = 'fixed'
        heartLayer.style.top = '0'
        heartLayer.style.left = '0'
    }


    init() {
        this.configFrame.height = innerHeight * 2
        this.configFrame.width = innerWidth * 2

        let heartLayer = document.createElement("canvas")
        this.updateFrameAttribute(heartLayer)
        document.documentElement.append(heartLayer)

        this.configHeart.timeLine = 0

        // Điền vào hình dạng được lưu trong bộ nhớ đệm
        for (let i = 0; i < this.configHeart.countHeart; i++) {
            let randomSize = randomInt(this.configHeart.sizeMin, this.configHeart.sizeMax)
            let x = randomInt(0, this.configFrame.width)
            let y = randomInt(this.configFrame.height * (1 - this.configHeart.heartRangeMax), this.configFrame.height * (1 - this.configHeart.heartRangeMin))
            this.heartBuffer.push({
                id: i,
                gravity: randomFloat(this.configHeart.gravityMin, this.configHeart.gravityMax),
                opacity: 0,
                opacityFinal: randomInt(this.configHeart.minOpacity, this.configHeart.maxOpacity), // minh bạch cuối cùng
                timeInit: randomInt(1, 500), // Sắp xếp ngẫu nhiên trái tim ban đầu  
                x, // Vị trí x
                y, // Vị trí y
                originalX: x,
                originalY: y,
                width: randomSize,  // heart Kích cỡ
                height: randomSize, // heart Kích cỡ
                colorH: randomInt(this.configHeart.hMin, this.configHeart.hMax)
            })
        }

        this.draw()

        document.documentElement.addEventListener('mousemove', event => {
            this.mouseX = event.x
            this.mouseY = event.y
        })
    }

    isMouseIsCloseToBox(box) {
        let distance = Math.sqrt(Math.pow(Math.abs(box.position.x / 2 - this.mouseX), 2) + Math.pow(Math.abs(box.position.y / 2 - this.mouseY), 2))
        return distance < 100
    }


    draw() {
        // Tạo dòng tham chiếu thời gian của riêng bạn，Loại bỏ sự nhầm lẫn về thời gian do chuyển đổi chương trình khi sử dụng thời gian hệ thống
        this.configHeart.timeLine = this.configHeart.timeLine + 1

        // create heart
        let canvasHeart = document.getElementById('heartLayer')
        let contextHeart = canvasHeart.getContext('2d')
        contextHeart.clearRect(0, 0, this.configFrame.width, this.configFrame.height)


        if (this.configFrame.bgColor) {
            contextHeart.fillStyle = this.configFrame.bgColor
            contextHeart.fillRect(0, 0, this.configFrame.width, this.configFrame.height)
        }


        this.heartBuffer.forEach(heart => {
            // Khi ra khỏi màn hình
            if (heart.y < -(heart.height)) {
                heart.y = heart.originalY
                heart.timeInit = this.configHeart.timeLine // Nút thời gian định vị lại
                heart.opacity = 0
            }

            // Khi độ trong suốt đạt đến độ trong suốt cuối cùng
            let timeGap = this.configHeart.timeLine - heart.timeInit // Độ trong suốt chỉ được tính khi thời gian là số dương
            if (timeGap > 0) {
                heart.opacity = heart.opacity * ((this.configHeart.timeLine - heart.timeInit) / 100)
            } else { 
                heart.opacity = 0
            }

            if (heart.opacity >= heart.opacityFinal) {
                heart.opacity = heart.opacityFinal // Hướng tới sự minh bạch cuối cùng
            }

            // 1/2 gt㎡  Quỹ đạo chuyển động
            // let movement = 1/2 * this.configHeart.gravity * Math.pow((new Date().getTime() - heart.timeInit)/1000,2)

            // speed = 1/2 gt
            let movement = 1 / 2 * heart.gravity * (this.configHeart.timeLine - heart.timeInit) / 300 * this.configHeart.flowDirection
            heart.y = heart.y - movement

            this.drawHeart(
                heart.x,
                heart.y,
                heart.width / 2,
                heart.height / 2,
                `hsl(${heart.colorH} ${this.configHeart.colorSaturate}% ${this.configHeart.colorLight}% / ${heart.opacity}%)`
            )
            heart.opacity = heart.opacity + this.configHeart.opacityGrowth

        })

        if (this.isPlaying) {
            window.requestAnimationFrame(() => {
                this.draw()
            })
        }
    }

    drawHeart(x, y, width, height, colorFill) {

        let canvasHeart = document.getElementById('heartLayer')
        let contextHeart = canvasHeart.getContext('2d')

        contextHeart.save()
        contextHeart.beginPath()
        let topCurveHeight = height * 0.3
        contextHeart.moveTo(x, y + topCurveHeight)
        // top left curve
        contextHeart.bezierCurveTo(
            x, y,
            x - width / 2, y,
            x - width / 2, y + topCurveHeight
        )
        // bottom left curve
        contextHeart.bezierCurveTo(
            x - width / 2, y + (height + topCurveHeight) / 2,
            x, y + (height + topCurveHeight) / 1.4,
            x, y + height
        )
        // bottom right curve
        contextHeart.bezierCurveTo(
            x, y + (height + topCurveHeight) / 1.8,
            x + width / 2, y + (height + topCurveHeight) / 2,
            x + width / 2, y + topCurveHeight
        )
        // top right curve
        contextHeart.bezierCurveTo(
            x + width / 2, y,
            x, y,
            x, y + topCurveHeight
        )
        contextHeart.closePath()
        contextHeart.fillStyle = colorFill
        contextHeart.fill()
        contextHeart.restore()
    }

}





/**
 * @returns {number}
 */
function randomDirection() {
    let random = Math.random()
    if (random > 0.5) {
        return 1
    } else {
        return -1
    }
}

/**
 * 生成随机整数
 * @param min
 * @param max
 * @returns {number}
 */
function randomInt(min, max) {
    return Number((Math.random() * (max - min) + min).toFixed(0))
}

/**
 * @param min
 * @param max
 * @returns {number}
 */
function randomFloat(min, max) {
    return Number(Math.random() * (max - min) + min)
}



let queryData = getSearchData()
console.log(queryData)
let animateHeartCanvas = new AnimateHeartCanvas(
    Number(queryData.hMin),
    Number(queryData.hMax),
    Number(queryData.countHeart),
    Number(queryData.sizeMin),
    Number(queryData.sizeMax),
    queryData.bgColor,
)

function getSearchData() {
    let searchString = location.search;
    if (searchString) {
        let obj = {};
        searchString = searchString.substring(1, searchString.length);
        let tempArray = searchString.split('&');
        tempArray.forEach(item => {
            obj[item.split('=')[0]] = decodeURIComponent(item.split('=')[1]);
        });
        return obj;
    } else {
        return false;
    }
}