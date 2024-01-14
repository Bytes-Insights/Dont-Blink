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
const x0_svg = svg.getBoundingClientRect().x; console.log(x0_svg);

console.log(hSVG, hAttrSVG);

const factor = hAttrSVG / hSVG;

const nPaths = paths.length;

const pathsData = [];

paths.forEach(path => {
    const d = path.getAttribute("d");
    pathsData.push(d);
})

console.log(paths, nPaths, pathsData);

const theta = Math.PI * 2 / (nPaths);

const points = [];

for (let i = 0; i < nPaths; i++) {

    const x2 = hAttrSVG / 2 + ( r * Math.sin(theta * i) ) * factor;
    const x1 = hAttrSVG / 2 + ( r0 * Math.sin(theta * i) ) * factor;

    const y2 = hAttrSVG / 2 + (r * Math.cos(theta * i)  )* factor;
    const y1 = hAttrSVG / 2 + (r0 * Math.cos(theta * i) ) * factor;

    points.push({x1,y1,x2,y2});

}

console.log(points);

paths.forEach( (path,i) => {

    const {x1, y1, x2, y2} = points[i];


    path.setAttribute("d", `M${x1} ${y1}L${x2} ${y2}`);
})