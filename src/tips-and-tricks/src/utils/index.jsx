export function isEmptyOrWhitespace(str) {
	return (
		str === null || (typeof str === 'string' && str.match(/^ *$/) !== null)
	);
}
