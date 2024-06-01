import {americanize, canadianize, version} from './replacements.js';

const pasteText = "Press Ctrl+V or ⌘+V to paste text to convert.";
const copyText = "Press Ctrl+C or ⌘+C to copy converted text.";
const readyText = "Text on clipboard ready to paste wherever you want.\r\n";
const id = s => document.getElementById(s);
const instructions = id('instructions');
const messages = id('messages');
const preview = id('preview');
const text = id('text');
const error = id('error');

const showError = message => {
    error.innerText = message;
    error.style.display = message ? 'block' : 'none';
};

let currentText = '';
const setText = html => {
    currentText = html;
    text.innerHTML = currentText;
    preview.innerHTML = currentText;
}
let toAmerican = false;

function convert(line, summary) {
    const regexes = toAmerican ? americanize : canadianize;
    for(const r of regexes) {
        for(let w of (line.match(r.re) ?? [])) {
            w = w.toLowerCase();
            summary.set(w, (summary.get(w) ?? 0) + 1);
        }
        line = line.replace(r.re, r.s);
    }
    return line;
}

text.focus();
text.onblur = e => { 
    setTimeout(() => {
        text.focus();
        window.getSelection().selectAllChildren(text);
    }, 100); 
};
text.oninput = e => { 
    text.innerHTML = currentText; 
    window.getSelection().selectAllChildren(text);
};
text.onclick = e => {
    setTimeout(() => {
        window.getSelection().selectAllChildren(text);
    }, 100);
}
text.onpaste = async e => {
    const pastedText = e.clipboardData.getData('text/plain');
    showError('');
    instructions.innerText = 'Working, please wait...';
    text.disabled = true;
    try {
        const span = document.createElement('span');
        const lines = pastedText.split(/\r?\n+/);
        const summary = new Map();
        setText(lines.map(line => {
            span.textContent = convert(line, summary);
            let html = span.innerHTML;
            return `<div>${html}</div>`
        }).join('\r\n'));
        messages.innerHTML = 'Replaced: ' + 
            [...summary.keys()].toSorted().map(k =>
                `${k}: ${summary.get(k)}x`
            ).join(', ');
        setTimeout(() => {
            instructions.innerText = copyText;
            window.getSelection().selectAllChildren(text);
        });
    } catch(e) {
        showError(e);
    } finally {
        text.disabled = false;
    }
};
text.oncopy = e => {
    setTimeout(() => {
        instructions.innerText = readyText + pasteText;
        messages.innerHTML = '';
        setText('&nbsp;');
    });
}

function initialize() {
    toAmerican = document.location.hash === '#american';
    if (!toAmerican) document.location.hash = '#canadian';
    showError('');
    setText('&nbsp;');
    instructions.innerText = pasteText;    
}
window.addEventListener('hashchange', _ => initialize());
initialize();