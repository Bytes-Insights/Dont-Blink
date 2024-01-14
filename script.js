const svg = document.querySelector(".dont-blink");
const paths = document.querySelectorAll(".dont-blink path");

const video = document.querySelector(".eye");

const videoRect = video.getBoundingClientRect();

const y0 = videoRect.y;
const h2 = videoRect.height / 2
const yc = y0 + h2;
const xc = videoRect.x + videoRect.width / 2;
const r = h2 * 0.9;
const r0 = r * 0.35;

const hSVG = +window.getComputedStyle(svg).height.slice(0,-2);
const hAttrSVG = +svg.getAttribute('height');
const x0_svg = svg.getBoundingClientRect().x; 

const factor = hAttrSVG / hSVG;

const nPaths = paths.length;

const pathsData = [];

paths.forEach(path => {
    const d = path.getAttribute("d");
    pathsData.push(d);
})

const theta = Math.PI * 2 / (nPaths);

const points = [];

for (let i = 0; i < nPaths; i++) {

    const x2 = hAttrSVG / 2 + ( r * Math.sin(theta * i) ) * factor;
    const x1 = hAttrSVG / 2 + ( r0 * Math.sin(theta * i) ) * factor;

    const y2 = ( hAttrSVG / 2 + (r * Math.cos(theta * i)  )* factor );
    const y1 = ( hAttrSVG / 2 + (r0 * Math.cos(theta * i) ) * factor );

    points.push({x1,y1,x2,y2});

}

paths.forEach( (path,i) => {

    const {x1, y1, x2, y2} = points[i];

    const xm = ( x1 + x2 ) / 2
    const ym = ( y1 + y2 ) / 2
    /*
    const xm1 = ( x1 + x2 ) / 3 + 3;
    const ym1 = ( y1 + y2 ) / 3;

    const xm2 = 2 * ( x1 + x2 ) / 3 - 3;
    const ym2 = 2 * ( y1 + y2 ) / 3;
    */

    const desiredPath = `M ${x1} ${y1} L ${xm} ${ym} L ${x2} ${y2}`;

    path.setAttribute("d", desiredPath);

    //path.currentPath = desiredPath;

})

const pd3 = d3.selectAll(".dont-blink path");

function bouge() {
    pd3.transition().duration(1500)
    .style("opacity", 1)

    pd3.transition().delay(1000 + ( Math.random() - .5) * 400).duration((d,i) => 3000 + (Math.random() - 0.5) * 500)
    .attrTween("d", function(d,i) {

        const d0 = d3.select(this).attr("d");

        return flubber.interpolate(d0, pathsData[i]);

    })

    d3.select('.eye').transition().duration(3000).style("opacity", 0);
}

bouge();
/*
pd3.attr("d", (d,i) => {

    const {x1, y1, x2, y2} = points[i];
    return `M${x1} ${y1}L${x2} ${y2}`;

})*/