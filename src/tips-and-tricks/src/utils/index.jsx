export function isEmptyOrWhitespace(str) {
	return (
		str === null || (typeof str === 'string' && str.match(/^ *$/) !== null)
	);
}

export function isInteger(str) {
	return Number.isInteger(Number(str)) && Number(str) >= 0;
}

export function getMonthName(monthNumber) {
	const month = [
		'January',
		'February',
		'March',
		'April',
		'May',
		'June',
		'July',
		'August',
		'September',
		'October',
		'November',
		'December',
	];

	return month[monthNumber - 1];
}

export function decode(str) {
	const txt = new DOMParser().parseFromString(str, 'text/html');
	return txt.documentElement.textContent;
}
