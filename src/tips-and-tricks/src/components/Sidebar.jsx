import SearchForm from './SearchForm';

export default function Sidebar() {
	return (
		<div className='pt-4 ps-2'>
			<SearchForm />
			<h1>Các chủ đề</h1>
			<h1>Bài viết nổi bật</h1>
			<h1>Đăng ký nhận tin mới</h1>
			<h1>Tag cloud</h1>
		</div>
	);
}
