import SearchForm from './SearchForm';
import {
	CategoriesWidget,
	FeaturedPostsWidget,
	RandomPostsWidget,
} from './widgets';

export default function Sidebar() {
	return (
		<div className='pt-4 ps-2'>
			<SearchForm />
			<CategoriesWidget />
			<FeaturedPostsWidget />
			<RandomPostsWidget />
			<h1>Đăng ký nhận tin mới</h1>
			<h1>Tag cloud</h1>
		</div>
	);
}
