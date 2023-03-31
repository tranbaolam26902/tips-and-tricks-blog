export function isEmptyOrWhitespace(str) {
	return (
		str === null || (typeof str === 'string' && str.match(/^ *$/) !== null)
	);
}

export function getMonthName(monthNumber) {
	const date = new Date();
	date.setMonth(monthNumber - 1);

	return date.toLocaleString('en-US', {
		month: 'long',
	});
}
