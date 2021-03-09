function getElementByInnerHtml(doms, str) {
    for (let index = 0; index < doms.length; index++) {
        const element = doms[index];
        if (element.innerHTML === str) {
            return element;
        }
        else if (element.children) {
            let result = getElementByInnerHtml(element.children, str);
            if (result != null) {
                return result;
            }
        }
    }

    return null;
}