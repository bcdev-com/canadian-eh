import {americanize, canadianize, version} from './replacements.js';

const id = s => document.getElementById(s);
const messages = id('messages');
const preview = id('preview');
const input = id('input');
const error = id('error');
const copy = id('copy');
const paste = id('paste');
let toAmerican = false;

const delay = ms => new Promise(r => setTimeout(r, ms));
const showError = message => {
    error.innerText = message;
    error.style.display = message ? 'block' : 'none';
};
const setPreview = html => preview.innerHTML = html;
const setInstructions = (ready, paste, copy, busy) => {
    id('ready-note').style.display = ready ? 'flex' : 'none';
    id('paste-note').style.display = paste ? 'flex' : 'none';
    id('copy-note').style.display = copy ? 'flex' : 'none';
    id('busy-note').style.display = busy ? 'flex' : 'none';
};

function convert(text, summary) {
    const regexes = toAmerican ? americanize : canadianize;
    for(const r of regexes) {
        for(let w of (text.match(r.re) ?? [])) {
            w = w.toLowerCase();
            summary.set(w, (summary.get(w) ?? 0) + 1);
        }
        text = text.replace(r.re, r.s);
    }
    return text;
}

input.focus();
input.onblur = async e => {
    await delay(100);
    input.focus(); 
};
input.oninput = async e => {
    const text = e.data;
    await delay(100);
    input.innerText = '';
    if (text.length > 4) await processText(text);
};
input.onpaste = async e => {
    e.preventDefault();
    e.stopPropagation();
    await processText(e.clipboardData.getData('text/plain'));
};
paste.onclick = async e => {
    let text = '';
    try {
        text = await navigator.clipboard.readText();
    } catch (e) {
        console.error(e);
        showError(e);
        return;
    }
    await processText(text);
};
    
async function processText(text) {
    try {
        showError('');
        setInstructions(false, false, false, true);
        input.disabled = true;
        await delay(0);
        const span = document.createElement('span');
        const summary = new Map();
        const newText = convert(text, summary);
        const html = newText.split(/\r?\n+/).map(line => {
            span.textContent = line;
            let html = span.innerHTML;
            return `<div>${html}</div>`
        }).join('\r\n');
        setPreview(html);
        const replacements = 
            [...summary.keys()].toSorted().map(k =>
                `${k}: ${summary.get(k)}x`
            ).join(', ');
        messages.innerText = replacements === '' ? 
            'Nothing to replace.' : `Replaced: ${replacements}`;

        await delay(0);
        setInstructions(false, false, true, false);
    } catch(e) {
        console.log(e);
        showError(e);
    } finally {
        input.disabled = false;
    }
};

copy.onclick = e => {
    try {
        copyTextWith(text => navigator.clipboard.writeText(text));
    } catch (e) {
        console.error(e);
        showError(e);
    }
};
input.oncopy = e => {
    e.preventDefault()
    e.stopPropagation();
    copyTextWith(text => e.clipboardData.setData('text/plain', text));
};
function copyTextWith(thunk) {
    thunk(preview.textContent);
    setInstructions(true, true, false, false);
    messages.innerHTML = '';
    setPreview('&nbsp;');
}

function initialize() {
    toAmerican = document.location.hash === '#american';
    if (!toAmerican) document.location.hash = '#canadian';
    showError('');
    setPreview('&nbsp;');
    setInstructions(false, true, false, false);
    messages.innerHTML = '';   
}
window.addEventListener('hashchange', _ => initialize());
initialize();